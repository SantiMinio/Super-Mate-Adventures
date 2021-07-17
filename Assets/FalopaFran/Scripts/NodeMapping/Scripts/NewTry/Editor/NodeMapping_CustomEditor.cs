using UnityEngine;
using UnityEditor;

namespace Frano.NodeMapping
{
    [CustomEditor(typeof(NodeMapping))]
    public class NodeMapping_CustomEditor : Editor
    {
        SerializedProperty gridData;
        SerializedProperty floorMask;
        ResizableGrid rg;

        string newName = "";
        //LayerMask floorMask;
        NodeMapping nodeMap;

        private void OnEnable()
        {
            nodeMap = (NodeMapping)target;
            gridData = serializedObject.FindProperty("gridData");
            floorMask = serializedObject.FindProperty("floorMask");
            if (rg == null) rg = nodeMap.GetComponent<ResizableGrid>();

            
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(gridData, new GUIContent("gridData"));
            EditorGUILayout.PropertyField(floorMask, new GUIContent("floorMask"));
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }


        private void OnSceneGUI()
        {
            serializedObject.Update();
            var nodeMapping = (NodeMapping)target;

            Handles.BeginGUI();
            {
                GUILayout.BeginArea(new Rect(20, 20, 250, 400));
                {
                    var rect = EditorGUILayout.BeginVertical();
                    {
                        GUI.Box(rect, GUIContent.none);

                        EditorGUILayout.LabelField("NODE MAPPER");
                        nodeMap.gridData = (GridData) EditorGUILayout.ObjectField(nodeMap.gridData, typeof(GridData), false);

                        if (GUILayout.Button("New Data"))
                        {
                            GridData newData = CreateInstance<GridData>();
                            
                            string name = AssetDatabase.GenerateUniqueAssetPath("Assets/FalopaFran/Scripts/NodeMapping/Data/Grid/NewGridData.asset");
                            AssetDatabase.CreateAsset(newData, name);
                            AssetDatabase.SaveAssets();

                            nodeMap.gridData = newData;
                        }


                        if (nodeMap.gridData != null)
                        {
                            EditorGUILayout.BeginHorizontal();
                            newName = EditorGUILayout.TextField(newName);
                            if(GUILayout.Button("Rename"))
                            {
                                if (newName != "")
                                {
                                    string assetPath = AssetDatabase.GetAssetPath(nodeMap.gridData.GetInstanceID());
                                    AssetDatabase.RenameAsset(assetPath, newName);
                                    AssetDatabase.SaveAssets();
                                }
                                newName = "";
                            }

                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.PropertyField(floorMask, new GUIContent("Ground layer"));
                            

                            EditorGUILayout.LabelField("");
                            if (GUILayout.Button("Clear data"))
                            {
                                nodeMap.gridData = null;
                            }

                            if (GUILayout.Button("Delete data"))
                            {
                                string pathToDelete = AssetDatabase.GetAssetPath(nodeMap.gridData);
                                AssetDatabase.DeleteAsset(pathToDelete);
                            }

                            EditorGUILayout.LabelField("");
                            rg.gridWorldSize = EditorGUILayout.Vector2Field("GridSize: ", rg.gridWorldSize);
                            rg.nodeRadius = EditorGUILayout.FloatField("NodeRadius: ", rg.nodeRadius);
                            rg.OnUpdate();


                            if (GUILayout.Button("RayScan"))
                            {
                                nodeMap.gridData = RayScan.RaycastScan(rg.GetBaseGrid(), nodeMap.gridData, nodeMap.floorMask);
                                EditorUtility.SetDirty(nodeMap.gridData);
                                
                                Debug.Log(nodeMap.gridData.matrixNode);
                            }
                        }

                       

                    }
                    EditorGUILayout.EndVertical();

                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }

}
