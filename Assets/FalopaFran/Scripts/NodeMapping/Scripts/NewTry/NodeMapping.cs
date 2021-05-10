using UnityEngine;

namespace Frano.NodeMapping
{
    public class NodeMapping : MonoBehaviour
    {
        public GridData gridData;
        public LayerMask floorMask;


        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;
            
            if (gridData == null || gridData.allNodes == null || gridData.allNodes.Count <= 0)
                return;
        
            foreach (var item in gridData.allNodes)
            {
                Gizmos.DrawSphere(item.worldPosition, gridData.nodeRadious / 2);
            }
        }
    }
}
