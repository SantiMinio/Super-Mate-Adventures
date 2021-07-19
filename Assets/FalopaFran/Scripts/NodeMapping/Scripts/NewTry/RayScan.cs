using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Frano.NodeMapping
{
    public class RayScan 
    {
        public static GridData RaycastScan(FinalSizeGridData baseGridData, GridData data, LayerMask floorMask)
        {
            data.allNodes.Clear();
            data.gridSizeX = Mathf.RoundToInt(baseGridData.gridSizeX);
            data.gridSizeY = Mathf.RoundToInt(baseGridData.gridSizeY);
            data.gridWorldSize = baseGridData.gridWorldSize;
            data.nodeRadious = baseGridData.nodeRadious;
            data.grid = new Node[data.gridSizeX, data.gridSizeY];
            data.matrixNode = new Matrix<Node>(data.gridSizeX, data.gridSizeY);

            Queue<Node> doubles = new Queue<Node>(); 
            
            for (int i = 0; i < data.gridSizeX; i++)
            {
                for (int j = 0; j < data.gridSizeY; j++)
                {
                    RaycastHit[] hits;

                    Ray ray = new Ray(baseGridData.grid[i, j].worldPos, Vector3.down);

                    hits = Physics.RaycastAll(ray, 100,floorMask);
                    hits = hits.OrderByDescending(f => f.point.y).ToArray();

                    //Debug.Log(hits.Length);
                    
                    if (!hits.Any()) continue;
                    
                    Node n = new Node(hits[0].point + hits[0].normal, i, j);
                    data.grid[i, j] = n;
                    data.matrixNode[i, j] = n;
                    data.allNodes.Add(n);

                    if (hits.Length > 1)
                    {
                        for (int k = 1; k < hits.Length; k++)
                        {
                            if (Vector3.Distance(hits[k].point, hits[k - 1].point) >= data.nodeRadious * 2)
                            {
                                Node node = new Node(hits[k].point + hits[k].transform.up * .5f, i, j);
                                
                                doubles.Enqueue(node);
                                data.allNodes.Add(node);
                            }
                        }

                    }
                    
                }
            }

            if (doubles.Count > 0)
            {
                Matrix<Node> aux = new Matrix<Node>(data.gridSizeX, data.gridSizeY);
            
                for (int i = 0; i < data.gridSizeX; i++)
                {
                    for (int j = 0; j < data.gridSizeY; j++)
                    {
                    
                    
                        Node doubleHitNode = doubles.Dequeue();
                        aux[i, j] = doubleHitNode;
                    }
                }

                data.matrixNode_doubles = aux;    
            }

            return data;
        }
    }
}


