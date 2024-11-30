using System.Collections.Generic;
using UnityEngine;

namespace CAD
{
    public class StateMachine
    {
        private List<IState> m_States = new();
        private IState m_CurrentState;
        private SmartTank m_TankAI;

        public StateMachine(SmartTank tankAI)
        {
            m_TankAI = tankAI;
        }

        public void Start()
        {
            m_CurrentState = m_States[0];
            m_CurrentState.OnStateEnter(m_TankAI);
        }

        public void Update()
        {
            m_CurrentState.OnStateUpdate(m_TankAI);

            foreach (Transition transition in m_CurrentState.GetTransitions())
            {
                if (transition.Condition(m_TankAI))
                {
                    SwitchState(transition.TargetState);
                    return;
                }
            }
        }

        public void AddState(IState state)
        {
            m_States.Add(state);
        }

        private void SwitchState(IState state)
        {
            if (m_CurrentState == state) return;

            m_CurrentState.OnStateExit(m_TankAI);
            m_CurrentState = state;
            m_CurrentState.OnStateEnter(m_TankAI);
        }
    }
}
