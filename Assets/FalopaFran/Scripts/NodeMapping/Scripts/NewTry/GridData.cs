using System.Collections.Generic;
using DevTools.NodeMapping.Scripts;
using UnityEngine;

namespace Frano.NodeMapping
{
    [CreateAssetMenu(menuName = "GridData")]
    [System.Serializable]
    public class GridData : ScriptableObject
    {
        public Vector2 gridWorldSize;
        public int gridSizeX, gridSizeY;
        public Node[,] grid;
        public Matrix<Node> matrixNode;
        public Matrix<Node> matrixNode_doubles;
        public List<Node> allNodes = new List<Node>();
        public List<Location_Pathfinding> nodeDirectionRegistry = new List<Location_Pathfinding>();
        public float nodeRadious;


        public int MaxSize
        {
            get
            {
                return gridSizeX * gridSizeY;
            }
        }
    }
}


