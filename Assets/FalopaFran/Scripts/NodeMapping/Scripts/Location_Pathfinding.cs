using UnityEngine;

namespace DevTools.NodeMapping.Scripts
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New location", menuName = "Pathfinding/Location", order = 0)]
    public class Location_Pathfinding : ScriptableObject
    {
        public Vector3 dir;
        public string lotacionName = "";
    }
}