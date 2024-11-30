using System;

namespace CAD
{
    public class Transition
    {
        public IState OriginState { get; private set; }
        public IState TargetState { get; private set; }
        public Func<SmartTank, bool> Condition { get; private set; }

        public Transition(IState originState, IState targetState, Func<SmartTank, bool> condition)
        {
            OriginState = originState;
            TargetState = targetState;
            Condition = condition;
        }
    }
}
