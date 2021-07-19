using System.Collections.Generic;
using System.Linq;
using DevTools.NodeMapping.Scripts;
using UnityEditor;
using UnityEngine;

namespace Frano.NodeMapping
{
    [CustomEditor(typeof(PlayableGrid))]
    public class PlayableGrid_CustomInspector : Editor
    {
        SerializedProperty gridData;
        SerializedProperty obstacleMask;
        SerializedProperty marker;
        SerializedProperty showNodes;
        SerializedProperty locationRegistry;
        SerializedProperty walkableRegions;
        SerializedProperty showWeight;
        SerializedProperty blurPenalty;
        SerializedProperty obstacleProximityPenalty;
        private SerializedProperty markerRadius;
        private PlayableGrid playableGrid;

        string locName = "";
        private bool showLocations;
        private bool test;
        private bool showMarker = false;

        public List<Location_Pathfinding> testList = new List<Location_Pathfinding>();
        private void OnEnable()
        {
            playableGrid = (PlayableGrid) target;
            gridData = serializedObject.FindProperty("gData");
            walkableRegions = serializedObject.FindProperty("walkableRegions");
            obstacleMask = serializedObject.FindProperty("obstacle");
            marker = serializedObject.FindProperty("marker");
            markerRadius = serializedObject.FindProperty("markerRadius");
            locationRegistry = serializedObject.FindProperty("locationRegistry");
            showNodes = serializedObject.FindProperty("showNodes");
            showWeight = serializedObject.FindProperty("showWeight");
            blurPenalty = serializedObject.FindProperty("blurPenalty");
            obstacleProximityPenalty = serializedObject.FindProperty("obstacleProximityPenalty");
        }

        public void OnSceneGUI()
        {
            serializedObject.Update();
            
            if (showMarker)
            {
                if (ClosestNodeToMarker() == null)
                {
                    Debug.Log("Marker sin nodos cerca!!");
                    Handles.color = Color.red;   
                    Handles.DrawSolidDisc(marker.vector3Value, Vector3.up, 4f);
                    marker.vector3Value = Handles.DoPositionHandle(marker.vector3Value, Quaternion.identity);
                    Handles.DrawWireDisc(marker.vector3Value, Vector3.up, markerRadius.floatValue);
                }
                else
                {
                    Handles.color = Color.green;   
                
                    Handles.DrawSolidDisc(marker.vector3Value, Vector3.up, 4f);
                    marker.vector3Value = Handles.DoPositionHandle(marker.vector3Value, Quaternion.identity);
                    Handles.DrawWireDisc(marker.vector3Value, Vector3.up, markerRadius.floatValue);

                    var proximityNodes = ProximityToMarkerNodes;

                    Handles.color = Color.blue;   
                
                    foreach (var node in proximityNodes)
                    {
                        Vector3 newPos = Handles.PositionHandle(node.worldPosition, Quaternion.identity);
                        Handles.DrawSolidDisc(newPos, Vector3.up, 2f);
                        if (newPos != node.worldPosition)
                        {
                            Undo.RecordObject(playableGrid.gData, "Move point");
                            playableGrid.MoveNode(node, newPos);
                        }
                    }

                    DeleteNodeWithMouseClick(proximityNodes);
                }
                
                
                
                
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(gridData, new GUIContent("gridData"));
            EditorGUILayout.PropertyField(obstacleMask, new GUIContent("obstacleMask"));
            EditorGUILayout.PropertyField(walkableRegions, new GUIContent("walkableRegions"));
            EditorGUILayout.PropertyField(showNodes, new GUIContent("showNodes"));
            EditorGUILayout.PropertyField(showWeight, new GUIContent("showWeight"));
            EditorGUILayout.PropertyField(blurPenalty, new GUIContent("blurPenalty"));
            EditorGUILayout.PropertyField(obstacleProximityPenalty, new GUIContent("obstacleProximityPenalty"));
            
            
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show marker"))
            {
                Debug.Log("Muestro marker");
                showMarker = true;
            }
            
            if (GUILayout.Button("Hide marker"))
            {
                Debug.Log("Escondo marker");
                showMarker = false;
            }
            EditorGUILayout.EndHorizontal();

            if (showMarker)
            {
                
                
                EditorGUILayout.PropertyField(markerRadius, new GUIContent("Marker radius"));
                EditorGUILayout.PropertyField(marker, new GUIContent("MarkerPos"));
                
                if (ClosestNodeToMarker() == null)
                {
                    EditorGUILayout.HelpBox("El marker no esta cerca de ningun nodo", MessageType.Error);
                }
                else
                {
                     EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add extra Node"))
                {
                    Debug.Log("agrego nodo");
                    Vector3 newPos = marker.vector3Value;
                    newPos += Vector3.up * 2;
                
                    playableGrid.AddNode(newPos);
                }
                if (GUILayout.Button("Delete Node"))
                {

                    Node node = ClosestNodeToMarker();

                    if (node != null)
                    {
                        Undo.RecordObject(playableGrid.gData, "Delete Node");
                        playableGrid.RemoveNode(node.worldPosition);
                    }
                }
                
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                
                locName = EditorGUILayout.TextField("Location Name", locName);

                if (locName == "")
                {
                    EditorGUILayout.HelpBox("Ponele nombre", MessageType.Warning);
                }
                EditorGUILayout.EndHorizontal();
                
                if (GUILayout.Button("New Direction"))
                {

                    Node node = ClosestNodeToMarker();

                    if (node != null)
                    {
                        Location_Pathfinding newLoc = CreateInstance<Location_Pathfinding>();
                            
                        string name = AssetDatabase.GenerateUniqueAssetPath("Assets/DevTools/NodeMapping/Data/Locations/" + locName + "_gLocation"+".asset");
                        AssetDatabase.CreateAsset(newLoc, name);
                        AssetDatabase.SaveAssets();

                        newLoc.lotacionName = locName;
                        

                        newLoc.dir = node.worldPosition;
                        
                        EditorUtility.SetDirty(newLoc);
                        playableGrid.AddNewLocation(newLoc);
                        EditorUtility.SetDirty(playableGrid);
                    }
                    
                    locName = "";
                }

                showLocations = GUILayout.Toggle(showLocations, "Show Locations");
                
                if (showLocations)
                {
                    playableGrid.UpdateLocationRegistry();
                    EditorGUILayout.PropertyField(locationRegistry, new GUIContent("locationRegistry"));
                }
                
                
                EditorGUILayout.Separator();
                
                
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                }

               
            }


            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Check vecinos"))
            {
                Debug.Log("registro vecinos");
                playableGrid.RegisterVecinos();
            }
            
            if (GUILayout.Button("Filter Vecinos"))
            {
                playableGrid.FilterDisableVecinos();
                Debug.Log("Filtro vecinos");
                
            }
            
            var style = new GUIStyle(GUI.skin.button);

            style.normal.textColor = Color.red;
            
            if (GUILayout.Button("Clear Vecinos", style))
            {
                playableGrid.ClearVecinos();
                
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        #region Auxiliar methods

        private void DeleteNodeWithMouseClick(IEnumerable<Node> proximityNodes)
        {
            Event e = Event.current;
            Vector3 mousePos = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.V)
            {
                test = true;
            }
            else if (e.type == EventType.KeyUp && e.keyCode == KeyCode.V)
            {
                test = false;
            }

            if (test && e.type == EventType.MouseDown && e.button == 1)
            {
                Debug.Log("entro");
                var selectedNode = proximityNodes.OrderBy(x => Vector3.Distance(x.worldPosition, mousePos)).First();


                if (selectedNode != null && Vector3.Distance(selectedNode.worldPosition, mousePos) <=
                    playableGrid.gData.nodeRadious * 3)
                {
                    Undo.RecordObject(playableGrid.gData, "Delete Node");
                    playableGrid.RemoveNode(selectedNode.worldPosition);
                }
            }
        }
        
        private IEnumerable<Node> ProximityToMarkerNodes
        {
            get
            {
                var proximityNodes = playableGrid.gData.allNodes.Where(n =>
                    Vector3.Distance(n.worldPosition, marker.vector3Value) <= markerRadius.floatValue);
                return proximityNodes;
            }
        }
        
        private Node ClosestNodeToMarker()
        {
            Node n = ProximityToMarkerNodes.OrderBy(x => Vector3.Distance(x.worldPosition, marker.vector3Value)).FirstOrDefault();

            
            return (n != null) ? n : null;
        }

        #endregion
        
       
    }    
}

