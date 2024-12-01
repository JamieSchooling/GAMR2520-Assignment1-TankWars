using System.Collections.Generic;
using UnityEngine;

namespace CAD
{
    [CreateAssetMenu(menuName = "AI/States/Chase State")]
    public class ChaseState : State
    {
        private Vector3 m_EnemyPos;

        public override void OnStateEnter(SmartTank tankAI)
        {
            // TODO: Implement OnStateEnter
        }

        public override void OnStateUpdate(SmartTank tankAI)
        {
            if (tankAI.EnemyTank)
            {
                tankAI.FollowPathToWorldPoint(tankAI.EnemyTank, 1f);
                m_EnemyPos = tankAI.EnemyTank.transform.position;
            }
        }

        public override void OnStateExit(SmartTank tankAI)
        {
            GameObject lastEnemyPos = new GameObject("LastEnemyPos");
            lastEnemyPos.transform.position = m_EnemyPos;
            tankAI.LastKnownEnemyPos = lastEnemyPos;
        }

        private void OnEnable()
        {
            Transitions = new()
            {
                new Transition("Low Resources", tankAI => tankAI.Health <= 30.0f || tankAI.Ammo <= 4.0f || tankAI.Fuel <= 50.0f),
                new Transition("Tank Lost", tankAI => !tankAI.EnemyTank),
                new Transition("Tank In Range", tankAI => Vector3.Distance(tankAI.transform.position, tankAI.EnemyTank.transform.position) < 25.0f)
            };
        }
    }
}
