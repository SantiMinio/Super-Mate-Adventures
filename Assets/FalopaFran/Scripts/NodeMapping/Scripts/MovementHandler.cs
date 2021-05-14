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

    #region Settings

    public enum  MovementType
    {
        WithForwardRotation,
        NoRotation
    }
    public void Init(Pathfinding pathfinding, EnemyDummy myDummy, Rigidbody rb)
    {
        _pathfinding = pathfinding;
        myRb = rb;
        //cc = charController;
        //turnSpeed = 20f;//state.turnSpeed;
        //speed = 10f; //state.speed;
        myTransform = myDummy.transform;
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
    
    public void GoTo(Vector3 pos)
    {
        if (moving) return;
        currentPosToGo = pos;
        currentType = MovementType.WithForwardRotation;
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
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(myTransform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
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
        this.turnSpeed = turnSpeed;
    }
}
