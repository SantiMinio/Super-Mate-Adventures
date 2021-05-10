using UnityEngine;

namespace Frano.NodeMapping
{
    //[ExecuteInEditMode]
    public class ResizableGrid : MonoBehaviour
    {
        public Vector2 gridWorldSize;
        public float nodeRadius;
        float nodeDiameter;
        int gridSizeX, gridSizeY;


        Vector3 prevPos;
        float prevNodeRadious;
        Vector2 prevGridWorldSize;

        GridPoint[,] grid;

        public FinalSizeGridData GetBaseGrid()
        {
            ForzeUpdate();
            FinalSizeGridData data = new FinalSizeGridData();

            data.gridSizeX = gridSizeX;
            data.gridSizeY = gridSizeY;
            data.grid = grid;
            data.gridWorldSize = gridWorldSize;
            data.nodeRadious = nodeRadius;
            grid = null;
            return data;

        }

        void ForzeUpdate()
        {
            CreateGrid();
            prevNodeRadious = nodeRadius;
            prevGridWorldSize = gridWorldSize;
            prevPos = transform.position;
        }
        
        public void OnUpdate()
        {
            if (prevNodeRadious != nodeRadius || prevGridWorldSize != gridWorldSize || prevPos != transform.position)
            {
                CreateGrid();
                prevNodeRadious = nodeRadius;
                prevGridWorldSize = gridWorldSize;
                prevPos = transform.position;
            }
        }

        void CreateGrid()
        {
            nodeDiameter = nodeRadius * 2;
            if (nodeDiameter > 0)
            {
                gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
                gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
                GetGrid();
            }
            else
            {
                Debug.Log("divide by 0!!");
            }
        }

        void GetGrid()
        {
            grid = new GridPoint[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    grid[x, y] = new GridPoint(worldPoint, x, y);
                }
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, .5f, gridWorldSize.y));

            if (grid != null && grid.Length > 0)
            {
                for (int i = 0; i < gridSizeX; i++)
                {
                    for (int j = 0; j < gridSizeY; j++)
                    {
                        Gizmos.DrawCube(grid[i, j].worldPos, new Vector3(.2f, .2f, .2f));
                    }
                }
            }
        }

    }
    [System.Serializable]
    public class GridPoint
    {
        public Vector3 worldPos;
        public int gridPosX, gridPosY;

        public GridPoint(Vector3 wPos, int X, int Y)
        {
            worldPos = wPos;
            gridPosX = X;
            gridPosY = Y;
        }
    }

    public struct FinalSizeGridData
    {
        public int gridSizeX;
        public int gridSizeY;
        public float nodeRadious;
        public GridPoint[,] grid;
        public Vector2 gridWorldSize;
    }

}

