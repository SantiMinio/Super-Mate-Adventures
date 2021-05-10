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
    
    
    
    private void Awake()
    {
        RegisterVecinos();
        FilterDisableVecinos();
       // StartCoroutine(RefreshGrid());
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
        foreach (var node in gData.allNodes)
        {
            List<Node> vecinos = new List<Node>();
            foreach (var n in gData.allNodes)
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
        // for (int i = 0; i < gData.allNodes.Count; i++)
        // {
        //     gData.allNodes[i].neighbours.Clear();
        // }
        
        registroVecinos.Clear();
    }
    
    
    public void FilterDisableVecinos()
    {
        List<Node> currentVecinos = new List<Node>();
        for (int i = 0; i < gData.allNodes.Count; i++)
        {
            Node cur = gData.allNodes[i];
            currentVecinos = registroVecinos[cur.worldPosition];
            if (cur.isDisabled)
            {
                registroVecinos[cur.worldPosition].Clear();
            }
            else
            {
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
    public Node NodeFromWorldPointbyDistance(Vector3 worldPos) { return gData.allNodes.OrderBy(n => Vector3.Distance(worldPos, n.worldPosition)).First(); }
    
    public List<Node> GetNeighbours(Node node) { return registroVecinos[node.worldPosition]; }

    private void OnDrawGizmos()
    {
        if(gData.allNodes != null && gData.allNodes.Count > 0 && registroVecinos.Count > 0)
        {
            
            
            foreach (var item in gData.allNodes)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(item.worldPosition, gData.nodeRadious);
                
                if(item.isDisabled) continue;
                
                if(registroVecinos.ContainsKey(item.worldPosition))
                    foreach (var v in registroVecinos[item.worldPosition])
                    {
                        if (v.isDisabled) continue;

                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(item.worldPosition, v.worldPosition);
                    }
            }
            
            foreach (var loc in locationRegistry)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(loc.dir, 20f);
            }
            
        }
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
