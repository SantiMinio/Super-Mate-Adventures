﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Frano.BehaviourEditor
{
    [CreateAssetMenu(menuName = "Editor/Nodes/Portal Node")]
    public class PortalNode : DrawNode
    {
        public override void DrawCurve(BaseNode n)
        {
            
        }

        public override void DrawWindow(BaseNode b)
        {
            b.stateRef.currentState = (State)EditorGUILayout.ObjectField(b.stateRef.currentState, typeof(State), false);
            b.isAssigned = b.stateRef.currentState != null;

            if (b.stateRef.previousState != b.stateRef.currentState)
            {
                b.stateRef.previousState = b.stateRef.currentState;
                BehaviourEditor.forceSetDirty = true;
            }
        }
    }

}

