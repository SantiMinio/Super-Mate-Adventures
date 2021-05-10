using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Node : IHeapItem<Node>
{
    public Vector3 worldPosition;
    public bool isDisabled = false;

    int heapIndex;

    public int gCost;
    public int hCost;
    public Node parent;
    public int gridX, gridY;

    public Node(Vector3 _worldPos, int gridPosX, int gridPosY)
    {
        worldPosition = _worldPos;
        gridX = gridPosX;
        gridY = gridPosY;
    }
    
    public Node(Vector3 _worldPos)
    {
        worldPosition = _worldPos;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}