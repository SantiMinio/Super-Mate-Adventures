using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Frano;
using System;
using System.IO;

namespace Frano.BehaviourEditor
{
    [CreateAssetMenu(menuName ="Editor/Nodes/State Node")]
    public class StateNode : DrawNode
    {        
        public override void DrawWindow(BaseNode b)
        {
            if(b.stateRef.currentState == null)
            {
                EditorGUILayout.LabelField("Add state to modify:");
            }
            else
            {
                if(!b.collapse)
                {
                    
                }
                else
                {
                    b.windowRect.height = 100;
                }

                b.collapse = EditorGUILayout.Toggle(" ", b.collapse);
            }

            b.stateRef.currentState = (State)EditorGUILayout.ObjectField(b.stateRef.currentState, typeof(State), false);

            if(b.previousCollapse != b.collapse)
            {
                b.previousCollapse = b.collapse;
            }

            if (b.stateRef.previousState != b.stateRef.currentState)
            {
                //b.serializedState = null;
                b.isDuplicate = BehaviourEditor.settings.currentGraph.IsStateNodeDuplicate(b);
                b.stateRef.previousState = b.stateRef.currentState;

                if(!b.isDuplicate)
                {
                    Vector3 pos = new Vector3(b.windowRect.x, b.windowRect.y, 0);
                    pos.x += b.windowRect.width * 2;

                    SetUpReordableLists(b);

                    for (int i = 0; i < b.stateRef.currentState.transitions.Count; i++)
                    {
                        pos.y += i * 100;
                        BehaviourEditor.AddTransitionNodeFromTransition(b.stateRef.currentState.transitions[i], b, pos); 
                    }

                    BehaviourEditor.forceSetDirty = true;
                }

            }

            if(b.isDuplicate)
            {
                EditorGUILayout.LabelField("State is a duplicate!");
                b.windowRect.height = 100;
                return;
            }

            if(b.stateRef.currentState != null)
            {
                b.isAssigned = true;
               

                if(!b.collapse)
                {
                    if(b.stateRef.serializedState == null)
                    {
                        SetUpReordableLists(b);
                    }

                    b.stateRef.serializedState.Update();
                    

                    EditorGUILayout.LabelField("");
                    b.stateRef.onFixedList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.stateRef.onUpdateList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.stateRef.onEnterList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.stateRef.onExitList.DoLayoutList();

                    b.stateRef.serializedState.ApplyModifiedProperties();

                    float standard = 500;
                    standard += (b.stateRef.onUpdateList.count) * 20;
                    standard += (b.stateRef.onEnterList.count) * 20;
                    standard += (b.stateRef.onExitList.count) * 20;
                    standard += (b.stateRef.onFixedList.count) * 20;
                    b.windowRect.height = standard;
                }
            }
            else
            {
                b.isAssigned = false;
            }
        }

        void SetUpReordableLists(BaseNode b)
        {
            b.stateRef.serializedState = new SerializedObject(b.stateRef.currentState);

            b.stateRef.onFixedList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onFixed"), true, true, true, true);
            b.stateRef.onUpdateList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onUpdate"), true, true, true, true);
            b.stateRef.onEnterList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onEnter"), true, true, true, true);
            b.stateRef.onExitList = new ReorderableList(b.stateRef.serializedState, b.stateRef.serializedState.FindProperty("onExit"), true, true, true, true);

            HandleReordableList(b.stateRef.onFixedList, "On Fixed");
            HandleReordableList(b.stateRef.onUpdateList, "On Update");
            HandleReordableList(b.stateRef.onEnterList, "On Enter");
            HandleReordableList(b.stateRef.onExitList, "On Exit");
        }

        void HandleReordableList(ReorderableList list, string targetName)
        {
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, targetName);
            };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };
        }

        public override void DrawCurve(BaseNode b)
        {

        }

        public Transition AddTransition(BaseNode b)
        {
            return  b.stateRef.currentState.AddTransition();
        }

        public void ClearReferences()
        {
           // BehaviourEditor.ClearWindowsFromList(dependecies);
           // dependecies.Clear();
        }


    }
}


