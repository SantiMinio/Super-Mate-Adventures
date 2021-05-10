using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano.NodeMapping
{
    public class ReconstructNodeGrid
    {
        public static Node[,] ReconstructGrid(List<Node> linearGrid, GridData gData)
        {
            Node[,] reconstructedGrid = new Node[gData.gridSizeX, gData.gridSizeY];

            //for (int i = 0; i < linearGrid.Count; i++)
            //{
            //    Node newNode = new Node(linearGrid[i].worldPosition, linearGrid[i].gridX, linearGrid[i].gridY);
            //    reconstructedGrid[newNode.gridX, newNode.gridY] = newNode;
            //}



            
            return reconstructedGrid;
        }
    }
}


