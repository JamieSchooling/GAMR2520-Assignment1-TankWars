using UnityEngine;

namespace CAD
{
    public class ChaseState : IState
    {
        private Vector3 m_EnemyPos;

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
