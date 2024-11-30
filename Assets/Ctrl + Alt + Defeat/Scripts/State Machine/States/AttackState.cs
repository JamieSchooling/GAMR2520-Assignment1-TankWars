using System.Collections.Generic;

namespace CAD
{
    public class AttackState : IState
    {
        private List<Transition> m_Transitions = new();

        public List<Transition> GetTransitions()
        {
            return m_Transitions;
        }

        public void OnStateEnter(SmartTank tankAI)
        {
            // TODO: Implement OnStateEnter
        }

        public void OnStateUpdate(SmartTank tankAI)
        {
            if (tankAI.EnemyTank) tankAI.TurretFireAtPoint(tankAI.EnemyTank);
        }

        public void OnStateExit(SmartTank tankAI)
        {
            // TODO: Implement OnStateExit
        }
    }
}
