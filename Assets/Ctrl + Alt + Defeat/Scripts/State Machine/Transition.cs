using System;

namespace CAD
{
    public class Transition
    {
        public Func<SmartTank, bool> Condition { get; private set; }
        public IState TargetState { get; private set; }

        public Transition(IState targetState, Func<SmartTank, bool> condition)
        {
            TargetState = targetState;
            Condition = condition;
        }
    }
}
