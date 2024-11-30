using System.Collections.Generic;
using UnityEngine;

namespace CAD
{
    public class ChaseState : IState
    {
        private Vector3 m_EnemyPos;

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
            if (tankAI.EnemyTank)
            {
                tankAI.FollowPathToWorldPoint(tankAI.EnemyTank, 1f);
                m_EnemyPos = tankAI.EnemyTank.transform.position;
            }
        }

        public void OnStateExit(SmartTank tankAI)
        {
            GameObject lastEnemyPos = new GameObject("LastEnemyPos");
            lastEnemyPos.transform.position = m_EnemyPos;
            tankAI.LastKnownEnemyPos = lastEnemyPos;
        }
    }
}
