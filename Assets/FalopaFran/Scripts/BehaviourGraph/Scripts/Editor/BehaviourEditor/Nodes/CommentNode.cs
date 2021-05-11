using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano.BehaviourEditor
{
    [CreateAssetMenu(menuName = "Editor/Comment Node")]
    public class CommentNode : DrawNode
    {
        public override void DrawCurve(BaseNode n)
        {
            
        }

        public override void DrawWindow(BaseNode b)
        {
            b.comment = GUILayout.TextArea(b.comment, 200);
        }
    }
}
