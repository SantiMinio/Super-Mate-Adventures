using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;

    public PlayableGrid grid;
    void Awake(){requestManager = GetComponent<PathRequestManager>(); }
    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPointbyDistance(startPos);
        Node targetNode = grid.NodeFromWorldPointbyDistance(targetPos);

        if (!startNode.isDisabled && !targetNode.isDisabled)
        {
            Heap<Node> openSet = new Heap<Node>(grid.gData.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    
                    if (neighbour.isDisabled || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        float heightDelta = 0f; 

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            heightDelta = path[i - 1].worldPosition.y - path[i].worldPosition.y;

            if (directionNew != directionOld || Mathf.Abs(heightDelta) >= .1f)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    public IEnumerable<Vector3> GetNodesBetweenUnitAndTarget(Vector3 myPos, Vector3 targetPos, float maxDistanceUnitToPosiblePos, int maxAmount)
    {
        IEnumerable<Vector3>  posiblePos = grid.gData.allNodes
                .Where(x =>Vector3.Distance(x.worldPosition, myPos) <= maxDistanceUnitToPosiblePos)
                .OrderBy(t =>  Vector3.Distance(t.worldPosition, (myPos + (targetPos - myPos) / 2)))
                .Select(t => t.worldPosition)
                .Where(n => IsPosReachable(myPos, n))
                .Take(maxAmount);
        
        return posiblePos;
    }
    
    public IEnumerable<Vector3> GetNodesCloseToPos(Vector3 unitPostion ,Vector3 targetPos, float maxDistanceToTargetPosiblePos, int maxAmount)
    {
        IEnumerable<Vector3>  posiblePos = grid.gData.allNodes
                .Where(x =>Vector3.Distance(x.worldPosition, targetPos) <= maxDistanceToTargetPosiblePos)
                .OrderBy(t =>  Vector3.Distance(t.worldPosition, unitPostion))
                .Select(t => t.worldPosition)
                .Where(n => IsPosReachable(targetPos, n))
                .Take(maxAmount);

        return posiblePos;
    }

    bool IsPosReachable(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPointbyDistance(startPos);
        Node targetNode = grid.NodeFromWorldPointbyDistance(targetPos);

        if (!startNode.isDisabled && !targetNode.isDisabled)
        {
            Heap<Node> openSet = new Heap<Node>(grid.gData.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return true;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    
                    if (neighbour.isDisabled || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        return false;
    }

    public IEnumerable<Vector3> SearchZoneNode(Vector3 originPos, Vector3 dirToSearch, float dist, float searchRadious)
    {
        Vector3 searchOrigin = originPos + dirToSearch * dist;
        
        return grid.gData.allNodes
            .Where(x => Vector3.Distance(searchOrigin, x.worldPosition) <= searchRadious)
            .Select(x => x.worldPosition);
    }
}