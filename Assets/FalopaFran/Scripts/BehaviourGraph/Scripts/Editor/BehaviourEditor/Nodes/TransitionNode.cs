using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Frano;


namespace Frano.BehaviourEditor
{[CreateAssetMenu(menuName = "Editor/Transition Node")]
    public class TransitionNode : DrawNode
    {
        public void Init(StateNode enterState, Transition transition)
        {

        }

        public override void DrawWindow(BaseNode b)
        {
            EditorGUILayout.LabelField("");
            BaseNode enterNode = BehaviourEditor.settings.currentGraph.GetNodeWithIndex(b.enterNode);

            if(enterNode == null)
            {
                return;
            }

            if(enterNode.stateRef.currentState == null)
            {
                BehaviourEditor.settings.currentGraph.DeleteNode(b.id);
                return;
            }

            Transition transition = enterNode.stateRef.currentState.GetTransition(b.transRef.transitionId);

            if (transition == null)
                return;

            transition.condition = 
                (Condition)EditorGUILayout.ObjectField(transition.condition
                , typeof(Condition), false);

            if (transition.condition == null)
            {
                EditorGUILayout.LabelField("No condition");
                b.isAssigned = false;
            }
            else
            {
                b.isAssigned = true;
                if(b.isDuplicate)
                {
                    EditorGUILayout.LabelField("Duplicate Condition");
                }
                else
                {
                    GUILayout.Label(transition.condition.description);

                    BaseNode targetNode = BehaviourEditor.settings.currentGraph.GetNodeWithIndex(b.targetNode);

                    if(targetNode != null)
                    {
                        if (!targetNode.isDuplicate)
                            transition.targetState = targetNode.stateRef.currentState;
                        else
                            transition.targetState = null;
                    }
                    else
                    {
                        transition.targetState = null;
                    }
                }
            }

            if(b.transRef.previousCondition != transition.condition)
            {
                b.transRef.previousCondition = transition.condition;
                b.isDuplicate = BehaviourEditor.settings.currentGraph.IsTransitionDuplicate(b);
                if(!b.isDuplicate)
                {
                    BehaviourEditor.forceSetDirty = true;
                }

            }
        }

        public override void DrawCurve(BaseNode b)
        {
            Rect rect = b.windowRect;
            rect.y += b.windowRect.height * .5f;
            rect.width = 1;
            rect.height = 1;

            BaseNode e = BehaviourEditor.settings.currentGraph.GetNodeWithIndex(b.enterNode);
            if(e == null)
            {
                BehaviourEditor.settings.currentGraph.DeleteNode(b.id);
            }
            else
            {
                Color targetColor = Color.green;
                if (!b.isAssigned || b.isDuplicate)
                    targetColor = Color.red;

                Rect r = e.windowRect;
                BehaviourEditor.DrawNodeCurve(r, rect, true, targetColor);
            }

            if (b.isDuplicate)
                return;

            if(b.targetNode > 0)
            {
                BaseNode t = BehaviourEditor.settings.currentGraph.GetNodeWithIndex(b.targetNode);
                if (t == null)
                {
                    b.targetNode = -1;
                }
                else
                {
                    rect = b.windowRect;
                    rect.x += rect.width;
                    Rect endRect = t.windowRect;
                    endRect.x -= endRect.width * .5f;

                    Color targetColor = Color.green;

                    if(t.drawNode is StateNode)
                    {
                        if (!b.isAssigned || b.isDuplicate)
                            targetColor = Color.red;
                    }
                    else
                    {
                        if (!t.isAssigned)
                            targetColor = Color.red;
                        else
                            targetColor = Color.yellow;
                    }


                    BehaviourEditor.DrawNodeCurve(rect, endRect,false, targetColor);
                }
            }
        }

    }
}

