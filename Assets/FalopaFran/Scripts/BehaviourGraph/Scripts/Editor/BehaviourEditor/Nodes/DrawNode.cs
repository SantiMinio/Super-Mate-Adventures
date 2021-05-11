using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano.BehaviourEditor
{
    public abstract class DrawNode : ScriptableObject
    {
        public abstract void DrawWindow(BaseNode n);
        public abstract void DrawCurve(BaseNode n);
    }
}

