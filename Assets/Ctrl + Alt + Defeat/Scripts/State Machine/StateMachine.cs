using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CAD
{
    public class StateMachine
    {
        private List<State> m_States = new();
        private List<Transition> m_Transitions = new();
        private State m_CurrentState;
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

            //foreach (Transition transition in m_Transitions.Where(t => t.OriginState == m_CurrentState))
            //{
            //    if (transition.Condition(m_TankAI))
            //    {
            //        SwitchState(transition.TargetState);
            //        return;
            //    }
            //}
        }

        public void AddState(State state)
        {
            m_States.Add(state);
        }
        
        public void AddTransition(string name, Func<SmartTank, bool> condition)
        {
            AddTransition(new Transition(name, condition));
        }

        public void AddTransition(Transition transition) 
        {
            m_Transitions.Add(transition);
        }

        private void SwitchState(State state)
        {
            if (m_CurrentState == state) return;

            m_CurrentState.OnStateExit(m_TankAI);
            m_CurrentState = state;
            m_CurrentState.OnStateEnter(m_TankAI);
        }
    }
}
