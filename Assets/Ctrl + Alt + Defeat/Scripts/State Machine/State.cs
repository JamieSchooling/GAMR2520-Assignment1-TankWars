using System.Collections.Generic;
using UnityEngine;

namespace CAD
{
    public abstract class State : ScriptableObject
    {
        public List<Transition> Transitions { get; protected set; }

        public abstract void OnStateEnter(SmartTank tankAI);
        public abstract void OnStateUpdate(SmartTank tankAI);
        public abstract void OnStateExit(SmartTank tankAI);
    }
}
