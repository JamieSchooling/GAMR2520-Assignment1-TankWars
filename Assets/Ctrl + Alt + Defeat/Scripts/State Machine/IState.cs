using System.Collections.Generic;

namespace CAD
{
    public interface IState
    {
        List<Transition> GetTransitions();
        void OnStateEnter(SmartTank tankAI);
        void OnStateUpdate(SmartTank tankAI);
        void OnStateExit(SmartTank tankAI);
    }
}
