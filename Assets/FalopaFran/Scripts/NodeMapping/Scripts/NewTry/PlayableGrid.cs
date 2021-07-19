using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Frano.NodeMapping;
using System.Linq;
using DevTools.NodeMapping.Scripts;


public class PlayableGrid : MonoBehaviour
{
    public GridData gData;
    [SerializeField] LayerMask obstacle;
    [SerializeField] Vector3 marker;
    Node[,] playableGrid;
    [SerializeField] private float markerRadius;

    [SerializeField] public List<Location_Pathfinding> locationRegistry = new List<Location_Pathfinding>();
    Dictionary<Vector3, List<Node>> registroVecinos = new Dictionary<Vector3, List<Node>>();

    private float refreshGridCounter = 0;
    
    
    public TerrainType[] walkableRegions;
    LayerMask walkableMask;
    [SerializeField] private int obstacleProximityPenalty = 33;
    [SerializeField] private int blurPenalty;
    Dictionary<int,int> walkableRegionsDictionary = new Dictionary<int, int>();
    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;
    
    private void Awake()
    {
        foreach (TerrainType region in walkableRegions) {
            walkableMask.value |= region.terrainMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value,2),region.terrainPenalty);
        }

       
        FilterDisabledInMatrix();
        
        RegisterVecinos();
        FilterDisableVecinos();
        SetWeights();

        BlurPenaltyMap(blurPenalty);
        
        // StartCoroutine(RefreshGrid());
        
        
    }

    private void FilterDisabledInMatrix()
    {
        for (int i = 0; i < gData.matrixNode.Width; i++)
        {
            for (int j = 0; j < gData.matrixNode.Height; j++)
            {
                gData.matrixNode[i,j].isDisabled = CheckDisableNode(gData.matrixNode[i, j]);
            }
        }
    }

    private void SetWeights()
    {
        for (int i = 0; i < gData.matrixNode.Width; i++)
        {
            for (int j = 0; j < gData.matrixNode.Height; j++)
            {
                if(gData.matrixNode[i,j].worldPosition == Vector3.zero) continue;
            
                int movementPenalty = 0;
            
                Ray ray = new Ray(gData.matrixNode[i,j].worldPosition, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit, 100, walkableMask)) {
                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }

                if (gData.matrixNode[i, j].isDisabled)
                {
                    movementPenalty += obstacleProximityPenalty;
                }
                
                
                gData.matrixNode[i,j].movementPenalty = movementPenalty;
                //Debug.Log(gData.matrixNode[i,j].movementPenalty + "a ver que onbda?");
            }
        }
    }

    private IEnumerator RefreshGrid()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < gData.allNodes.Count; i++)
            {
                gData.allNodes[i].isDisabled = CheckDisableNode(gData.allNodes[i]);
            }
            
            List<Node> currentVecinos = new List<Node>();
            for (int i = 0; i < gData.allNodes.Count; i++)
            {
                Node cur = gData.allNodes[i];
                currentVecinos = registroVecinos[cur.worldPosition];
                // if (cur.isDisabled)
                // {
                //     registroVecinos[cur.worldPosition].Clear();
                // }
                //else
                //{
                    //Check si vecinos estan en linea de vision con un rayo
                    for (int j = currentVecinos.Count - 1; j >= 0; j--)
                    {
                        Vector3 dir = (currentVecinos[j].worldPosition - cur.worldPosition).normalized;
                        Ray r = new Ray(cur.worldPosition, dir);

                        if (Physics.Raycast(r, gData.nodeRadious * 2.5f))
                        {
                            currentVecinos.Remove(currentVecinos[j]);
                        }
                    }     
                //}
            }
            
            yield return new WaitForEndOfFrame();
            
        }
    }

    bool CheckDisableNode(Node n)
    {
         return Physics.CheckSphere(n.worldPosition, gData.nodeRadious, obstacle);
    }
    public void RegisterVecinos()
    {
        registroVecinos.Clear();
        foreach (var node in gData.matrixNode)
        {
            List<Node> vecinos = new List<Node>();
            foreach (var n in gData.matrixNode)
            {
                if (node.worldPosition == n.worldPosition)
                    continue;

                float dist = Vector3.Distance(node.worldPosition, n.worldPosition);

                if (dist <= gData.nodeRadious * 2.5f)
                {
                    vecinos.Add(n);
                }
            }
            
            if(!registroVecinos.ContainsKey(node.worldPosition))
                registroVecinos.Add(node.worldPosition, vecinos);

            
            node.isDisabled = CheckDisableNode(node);   
        }
    }

    public void ClearVecinos()
    {
        registroVecinos.Clear();
    }

    public void FilterDisableVecinos()
    {
        List<Node> currentVecinos = new List<Node>();
        foreach (var node in gData.matrixNode)
        {
            currentVecinos = registroVecinos[node.worldPosition];
            if (node.isDisabled)
            {
                registroVecinos[node.worldPosition].Clear();
            }
            else
            {
                //Check si vecinos estan en linea de vision con un rayo
                for (int j = currentVecinos.Count - 1; j >= 0; j--)
                {
                    Vector3 dir = (currentVecinos[j].worldPosition - node.worldPosition).normalized;
                    Ray r = new Ray(node.worldPosition, dir);

                    if (Physics.Raycast(r, gData.nodeRadious * 2.5f))
                    {
                        currentVecinos.Remove(currentVecinos[j]);
                    }
                }
            }
        }
    }
    
    void BlurPenaltyMap(int blurSize) {
        
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;
    
        int[,] penaltiesHorizontalPass = new int[gData.matrixNode.Width,gData.matrixNode.Height];
        int[,] penaltiesVerticalPass = new int[gData.matrixNode.Width,gData.matrixNode.Height];
    
        for (int y = 0; y < gData.matrixNode.Height; y++) {
            for (int x = -kernelExtents; x <= kernelExtents; x++) {
                int sampleX = Mathf.Clamp (x, 0, kernelExtents);
                penaltiesHorizontalPass [0, y] += gData.matrixNode [sampleX, y].movementPenalty;
            }
    
            for (int x = 1; x < gData.matrixNode.Width; x++) {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gData.matrixNode.Width);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gData.matrixNode.Width-1);
    
                penaltiesHorizontalPass [x, y] = penaltiesHorizontalPass [x - 1, y] - gData.matrixNode [removeIndex, y].movementPenalty + gData.matrixNode [addIndex, y].movementPenalty;
            }
        }
			 
        for (int x = 0; x < gData.matrixNode.Width; x++) {
            for (int y = -kernelExtents; y <= kernelExtents; y++) {
                int sampleY = Mathf.Clamp (y, 0, kernelExtents);
                penaltiesVerticalPass [x, 0] += penaltiesHorizontalPass [x, sampleY];
            }
    
            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, 0] / (kernelSize * kernelSize));
            gData.matrixNode [x, 0].movementPenalty = blurredPenalty;
    
            for (int y = 1; y < gData.matrixNode.Height; y++) {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gData.matrixNode.Height);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gData.matrixNode.Height-1);
    
                penaltiesVerticalPass [x, y] = penaltiesVerticalPass [x, y-1] - penaltiesHorizontalPass [x,removeIndex] + penaltiesHorizontalPass [x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, y] / (kernelSize * kernelSize));
                gData.matrixNode [x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax) {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin) {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    
    }

    public void UpdateLocationRegistry()
    {
        for (int i = 0; i < gData.nodeDirectionRegistry.Count; i++)
        {
            if (gData.nodeDirectionRegistry[i] == null) gData.nodeDirectionRegistry.Remove(gData.nodeDirectionRegistry[i]);
        }

        locationRegistry = gData.nodeDirectionRegistry;
    }
    
    public void AddNewLocation(Location_Pathfinding newLoc)
    {
        gData.nodeDirectionRegistry.Add(newLoc);
        UpdateLocationRegistry();
    }
    
    public void MoveNode(Node n, Vector3 newPos)
    {
        n.worldPosition = newPos;
    }

    public void AddNode(Vector3 nodePos)
    {
        Node newNode = new Node(nodePos);
        gData.allNodes.Add(newNode);
    }
    
    public void RemoveNode(Vector3 nodePos)
    {
        for (int i = gData.allNodes.Count - 1; i >= 0; i--)
        {
            if (gData.allNodes[i].worldPosition == nodePos) gData.allNodes.Remove(gData.allNodes[i]);
        }
    }
    public Node NodeFromWorldPointbyDistance(Vector3 worldPos) { return gData.matrixNode.OrderBy(n => Vector3.Distance(worldPos, n.worldPosition)).First(); }
    public IEnumerable<Node> NodesFromWorldPointbyDistance(Vector3 worldPos) { return gData.matrixNode.OrderBy(n => Vector3.Distance(worldPos, n.worldPosition)); }

    public List<Node> GetNeighbours(Node node) { return registroVecinos[node.worldPosition]; }

    public bool showNodes;
    public bool showWeight;
    private void OnDrawGizmos()
    {
        if (gData == null) return;

        if (gData.matrixNode.Count() > 0 && registroVecinos.Count > 0)
        {

            if (showWeight)
            {

                foreach (var mtxNode in gData.matrixNode)
                {
                    if (mtxNode.worldPosition == Vector3.zero) continue;

                    Gizmos.color = Color.Lerp(Color.white, Color.black,
                        Mathf.InverseLerp(penaltyMin, penaltyMax, mtxNode.movementPenalty));
                    Gizmos.color = (!mtxNode.isDisabled) ? Gizmos.color : Color.red;
                    Gizmos.DrawCube(mtxNode.worldPosition, Vector3.one * (1.5f));
                }
            }


            if (showNodes)
            {
                foreach (var item in gData.matrixNode)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(item.worldPosition, gData.nodeRadious);

                    if (item.isDisabled) continue;

                    if (registroVecinos.ContainsKey(item.worldPosition))
                        foreach (var v in registroVecinos[item.worldPosition])
                        {
                            if (v.isDisabled) continue;

                            Gizmos.color = Color.yellow;
                            Gizmos.DrawLine(item.worldPosition, v.worldPosition);
                        }
                }
            }

        }

        foreach (var loc in locationRegistry)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(loc.dir, 20f);
        }
    }
    
    [System.Serializable]
    public class TerrainType {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

    #region Deprecated- for simple grid

    //esta va a quedar para la forma simple de grilla
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gData.gridWorldSize.x / 2) / gData.gridWorldSize.x;
        float percentY = (worldPosition.z + gData.gridWorldSize.y / 2) / gData.gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gData.gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gData.gridSizeY - 1) * percentY);
        return playableGrid[x, y];
    }
    
    public List<Node> GetNeighboursWithArray(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gData.gridSizeX && checkY >= 0 && checkY < gData.gridSizeY)
                {
                    neighbours.Add(playableGrid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    #endregion
    
}
