using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using FranoW;
using Debug = UnityEngine.Debug;
using System.Linq;

[System.Serializable]
public class MovementHandler
{
    public Transform target;
    float turnSpeed;
    float speed;

    private Path auxPath;
    [SerializeField] float turnDistance;
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    
    [SerializeField] float minDistToChangeNode;
    [SerializeField] private float minDistToReachPos;
    float initSpeed;
    public Vector3[] path;
    public Vector3 currentWaypoint;
    public int targetIndex;
    //CharacterController cc;
    [SerializeField] Transform myTransform;

    [SerializeField] Transform groundCheck = null;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float gravityScaler = 1f;
    [SerializeField] float fall_Scaler = 5f;
    [SerializeField] float groundDistance = .4f;
    [SerializeField] float jumpDistance = 200f;
    [SerializeField] LayerMask groundMask;
    public bool grounded;
    
    public Vector3 velocity;
    //[SerializeField] private ParabolicShooter _parabolicShooter;
    public event Action OnReachDestination; 
    public bool moving;
    public bool jumping;
    public bool cantReachToDesiredPosition = false;
    MovementType currentType;

    public Vector3 auxiliarPosition = new Vector3(-1,-1,-1);
    public Vector3 currentPosToGo;
    public Vector3 prevPosToGoThatCantGo;
    public Vector3 currentLandingSpot;


    public bool forceMove = false;
    
    private Pathfinding _pathfinding;

    private bool systemActive;
    public float JumpDistance => jumpDistance;
    public bool UnitIsMoving => moving;
    public bool UnitIsJumping => jumping;
    public bool CantReachToDesiredPosition => cantReachToDesiredPosition;

    public Rigidbody myRb;
    
    
    #region JumpControllerQueries

    // public bool IsUnitInJumpDistance() => _parabolicShooter.IsInJumpDistance(currentLandingSpot ,jumpDistance);
    // public bool IsUnitInJumpDistance(Vector3 targetPos) => _parabolicShooter.IsInJumpDistance(targetPos ,jumpDistance);
    // public bool CanIJumpFromTo(Vector3 initialPos, Vector3 targetPos) => _parabolicShooter.IsInJumpDistance(targetPos ,jumpDistance);
    // public bool AreThereObstaclesInJumpTrayectory() => _parabolicShooter.IsAnObstacleInTheWay(currentLandingSpot);

    #endregion

    public void StartUpdatePath()
    {
        myMono.StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath() {

        if (Time.timeSinceLevelLoad < .3f) {
            yield return new WaitForSeconds (.3f);
        }

        while (target == null)
        {
            yield return new WaitForEndOfFrame();
        }
            
        PathRequestManager.RequestPath (myRb.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true) {
            yield return new WaitForSeconds (minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                PathRequestManager.RequestPath (myRb.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }
    
    
    #region Settings

    public enum  MovementType
    {
        WithForwardRotation,
        NoRotation
    }

    private MonoBehaviour myMono;
    public void Init(Pathfinding pathfinding, EnemyDummy myDummy, Rigidbody rb)
    {
        _pathfinding = pathfinding;
        myRb = rb;
        //cc = charController;
        //turnSpeed = 20f;//state.turnSpeed;
        //speed = 10f; //state.speed;
        myTransform = myDummy.transform;
        myMono = myDummy;
        //_parabolicShooter = new ParabolicShooter(myTransform);
        
        //ResetAuxiliarPos();
        
        OnReachDestination += () => Stop();

        systemActive = true;
        initSpeed = speed;

        
    }

    #endregion

    #region Physics

    void CustomJumpFeel()
    {
        if (velocity.y < -2f)
        {
            gravityScaler = fall_Scaler;
        }
        else
        {
            gravityScaler = 1f;
        }
    }
    public void OnUpdate()
    { 
        if (!systemActive) return;
    }
    public void OnFixedUpdate()
    {
        if (!systemActive) return;
        
        InputMovementUpdate();


        //Gravity();
    }
    // private void Gravity()
    // {
    //     grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    //
    //     if (grounded && velocity.y < 0)
    //     {
    //         velocity.y = -2f;
    //     }
    //
    //     CustomJumpFeel();
    //     velocity = FranoW.Gravity.Apply(velocity, gravity, gravityScaler, cc);
    // }
    void InputMovementUpdate()
    {
        if(moving)
        {
             if(currentType == MovementType.WithForwardRotation)
                 FollowPath();
            else
                FolloWPathWithoutRotation();
        }
    }

    #endregion

    #region PublicMethods

    public IEnumerable<Vector3> SearchForPosibleJumpNodes() => 
        _pathfinding.GetNodesBetweenUnitAndTarget(myTransform.position, prevPosToGoThatCantGo, 300f, 3);
    public IEnumerable<Vector3> SearchForPosibleLandingNodes() => 
        _pathfinding.GetNodesCloseToPos(myTransform.position,currentLandingSpot, 200f,50);
    public IEnumerable<Vector3> SearchForPosibleLandingNodesFromPos(Vector3 pos) => 
        _pathfinding.GetNodesCloseToPos(pos,currentLandingSpot, 200f,50);


    private Vector3 prevPos;
    private float _count;
    public void CountDownToForceMove()
    {

        if (!forceMove || prevPos == myRb.position)
        {
            _count += Time.deltaTime;

            if (_count >= 3f)
            {
                forceMove = true;
            }
        }
        else
        {
            forceMove = false;
            _count = 0;
            prevPos = myRb.position;
        }
    }
    
    
    
    
    public void GoTo(Vector3 pos)
    {
        if (moving) return;
        currentPosToGo = pos;
        currentType = MovementType.WithForwardRotation;
        Debug.Log("?");
        PathRequestManager.RequestPath(myTransform.position, pos, OnPathFound);
    }
    public void GoToAuxiliarPos()
    {
        if (moving) return;
        currentType = MovementType.WithForwardRotation;
        PathRequestManager.RequestPath(myTransform.position, auxiliarPosition, OnPathFound);
    }
    // public void Jump()
    // {
    //     currentType = MovementType.NoRotation;
    //     jumping = true;
    //     OnReachDestination += OnFinishJump;
    //     OnPathFound(_parabolicShooter.GetParabolePath(currentLandingSpot), true);
    // }

    public void Stop()
    {
        myRb.velocity = Vector3.zero;
        moving = false;
        currentWaypoint = Vector3.zero;
        path = null;
        velocity = Vector3.zero;
        
    }
    public void StopMovementSystem()
    {
        //cc.enabled = false;
        Stop();
        systemActive = false;
    }
    
    public void ResumeMovementSystem()
    {
        //cc.enabled = true;
        systemActive = true;
    }
    
    public void ResetAuxiliarPos(){auxiliarPosition = new Vector3(-1, -1, -1);}

    public void SpeedDown(float speedDown)
    {
        speed -= speedDown;
        if (speed < 1) speed = 1;
    }

    public void ResetSpeed() => speed = initSpeed;

    #endregion

    #region InternalMovementSystem
    // private void OnFinishJump()
    // {
    //     currentLandingSpot = new Vector3(-1, -1, -1); //reset landingSpot
    //     jumping = false;
    //     OnReachDestination -= OnFinishJump;
    // }

    // void OnPathFound(Vector3[] waypoints, bool pathSuccesful)
    // {
    //     if (pathSuccesful)
    //     {
    //         auxPath = new Path(waypoints, myRb.position, turnDistance, stoppingDst);
    //         
    //         myMono.StopCoroutine(FollowPathCorutine());
    //         myMono.StartCoroutine(FollowPathCorutine());
    //     }
    // }
    
    public float stoppingDst = 5;
    IEnumerator FollowPathCorutine() {

        bool followingPath = true;
        int pathIndex = 0;
        myTransform.LookAt (auxPath.lookPoints [0]);

        float speedPercent = 1;

        while (followingPath) {
            Vector2 pos2D = new Vector2 (myRb.position.x, myRb.position.z);
            while (auxPath.turnBoundaries [pathIndex].HasCrossedLine (pos2D)) {
                if (pathIndex == auxPath.finishLineIndex) {
                    followingPath = false;
                    break;
                } else {
                    pathIndex++;
                }
            }

            if (followingPath) {

                if (pathIndex >= auxPath.slowDownIndex && stoppingDst > 0) {
                    speedPercent = Mathf.Clamp01 (auxPath.turnBoundaries [auxPath.finishLineIndex].DistanceFromPoint (pos2D) / stoppingDst);
                    if (speedPercent < 0.01f) {
                        followingPath = false;
                    }
                }

                Quaternion targetRotation = Quaternion.LookRotation (auxPath.lookPoints [pathIndex] - myTransform.position);
                myRb.rotation = Quaternion.Lerp (myRb.rotation, targetRotation, Time.deltaTime * turnSpeed);
                MoveTowardsNextNode(myTransform.forward, speedPercent);
                //transform.Translate (Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }

            yield return null;

        }
    }
    
    
    
    void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(moving) return; 
       
        if (pathSuccessful && newPath.Length > 0)
        {
            cantReachToDesiredPosition = false;
            path = newPath;
            targetIndex = 0;
            moving = true;
            currentWaypoint = path[0];
        }else if (!pathSuccessful)
        {
            Rescate();
            cantReachToDesiredPosition = true;
            prevPosToGoThatCantGo = currentPosToGo;
            currentLandingSpot = currentPosToGo;
        }
    }
    void FolloWPathWithoutRotation()
    {
        Vector3 dir = GetDirToNextNode();
        if (NodeReached()) return;

        MoveTowardsNextNode(dir);
    }
    void FollowPath()
    {
        var dir = GetDirToNextNode();
        
        RotateForwardTowards(dir);

        if (NodeReached()) return;


        MoveTowardsNextNode(myTransform.forward);
    }
    private Vector3 GetDirToNextNode()
    {
        Vector3 dir = (currentWaypoint - myTransform.position).normalized;
        return dir;
    }
    private void MoveTowardsNextNode(Vector3 dir)
    {
        velocity = dir * speed;
        myRb.velocity = velocity;
    }

    private void MoveTowardsNextNode(Vector3 dir, float speedPercent)
    {
        velocity = dir * speed * speedPercent;
        myRb.velocity = velocity;
    }
    
    public void Rescate()
    {
        foreach (var node in Main.instance.GetPlayableGrid.NodesFromWorldPointbyDistance(myRb.position))
        {
            if (!node.isDisabled)
            {
                Debug.Log("ME RESCATO");
                Vector3 dir = (node.worldPosition - myRb.position).normalized;
                
                myRb.AddForce(300 * dir, ForceMode.Force);
                break;
            }
        }
        
    }
    private bool NodeReached()
    {
        //Debug.Log(Vector3.Distance(myTransform.position, currentPosToGo));

        // if (Vector3.Distance(myTransform.position, currentPosToGo) < minDistToReachPos)
        // {
        //     moving = false;
        //     
        //     Stop();
        //     OnReachDestination?.Invoke();
        //     return true;
        // }
        
        if (Vector3.Distance(myTransform.position, currentWaypoint) <= minDistToChangeNode)
        {
            
            targetIndex++;
            if (targetIndex >= path.Length)
            {
                moving = false;
                
                Stop();
                OnReachDestination?.Invoke();
                Debug.Log("Llegué");
                return true;
            }

            currentWaypoint = path[targetIndex];
        }

        return false;
    }
    void RotateForwardTowards(Vector3 dir)
    {
        myTransform.forward = Vector3.Lerp(myTransform.forward, new Vector3(dir.x, 0, dir.z), turnSpeed * Time.deltaTime);
    }

    #endregion
    
    public void OnDrawGizmos()
    {
        if (auxPath != null) {
                auxPath.DrawWithGizmos ();
        }


        if (auxPath != null)
        {
            for (int i = targetIndex; i < auxPath.lookPoints.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(auxPath.lookPoints[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(myTransform.position, auxPath.lookPoints[i]);
                }
                else
                {
                    Gizmos.DrawLine(auxPath.lookPoints[i - 1], auxPath.lookPoints[i]);
                }
            }
        }
    }

    public IEnumerable<Vector3> SearchForAnotherNodeZone(float distToSearch, Vector3 dirToSearch)
    {
        return _pathfinding.SearchZoneNode(myTransform.position,dirToSearch,distToSearch, 80f);
    }

    public void SetSpeeds(float turnSpeed, float moveSpeed)
    {
        speed = moveSpeed;
        initSpeed = speed;
        this.turnSpeed = turnSpeed;
    }
}
