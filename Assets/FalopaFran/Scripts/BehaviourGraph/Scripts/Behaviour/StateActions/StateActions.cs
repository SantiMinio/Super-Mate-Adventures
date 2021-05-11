﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    public abstract class StateActions : ScriptableObject
    {
        public abstract void Execute(StateManager states);
    }
}


