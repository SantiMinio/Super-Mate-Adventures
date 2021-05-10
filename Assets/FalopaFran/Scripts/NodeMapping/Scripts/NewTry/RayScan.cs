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

            List<Node> aditionalNodes = new List<Node>();

            for (int i = 0; i < data.gridSizeX; i++)
            {
                for (int j = 0; j < data.gridSizeY; j++)
                {
                    RaycastHit[] hits;

                    Ray ray = new Ray(baseGridData.grid[i, j].worldPos, Vector3.down);

                    hits = Physics.RaycastAll(ray, 100,floorMask);
                    hits = hits.OrderByDescending(f => f.point.y).ToArray();

                    Debug.Log(hits.Length);
                    
                    if (!hits.Any()) continue;
                    
                    Node n = new Node(hits[0].point + hits[0].normal, i, j);
                    data.grid[i, j] = n;
                    data.allNodes.Add(n);

                    if (hits.Length > 1)
                    {
                        for (int k = 1; k < hits.Length; k++)
                        {
                            if (Vector3.Distance(hits[k].point, hits[k - 1].point) >= data.nodeRadious * 2)
                            {
                                Node node = new Node(hits[k].point + hits[k].transform.up * .5f, i, j);
                                
                                data.allNodes.Add(node);
                            }
                        }

                    }
  


                    
                    //if (hitResult)
                    //{
                    //    Node n = new Node(hit.point + hit.transform.up * .5f, i, j);
                        
                    //    data.grid[i, j] = n;

                    //    data.allNodes.Add(n);
                    //    //n.isDisabled = Physics.CheckSphere(n.worldPosition, data.nodeRadius + (grid.nodeRadius / 1.5f), unwalkableMask); //pequeño offset para asegurarme de que no se coencte mal
                    //}
                }
            }
            //data.allNodes.AddRange(aditionalNodes);
            
            return data;
        }
    }
}


