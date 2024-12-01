using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CAD
{
    [CreateAssetMenu(menuName = "AI/States/Search State")]
    public class SearchState : State
    {
        private float m_CurrentTime;

        public override void OnStateEnter(SmartTank tankAI)
        {
            m_CurrentTime = 0;
        }

        public override void OnStateUpdate(SmartTank tankAI)
        {
            if (tankAI.LastKnownEnemyPos)
            {
                if (Vector3.Distance(tankAI.transform.position, tankAI.LastKnownEnemyPos.transform.position) < 5.0f)
                {
                    Destroy(tankAI.LastKnownEnemyPos);
                    tankAI.LastKnownEnemyPos = null;
                }
                tankAI.FollowPathToWorldPoint(tankAI.LastKnownEnemyPos, 1f);
            }
            else if (tankAI.VisibleConsumables.Count > 0)
            {
                GameObject consumable = tankAI.VisibleConsumables.First().Key;
                tankAI.FollowPathToWorldPoint(consumable, 1f);
                m_CurrentTime += Time.deltaTime;
            }
            else
            {
                tankAI.FollowPathToRandomWorldPoint(1f);
            }

            m_CurrentTime += Time.deltaTime;
            if (m_CurrentTime > 10)
            {
                tankAI.GenerateNewRandomWorldPoint();
                m_CurrentTime = 0;
            }
        }

        public override void OnStateExit(SmartTank tankAI)
        {
            // TODO: Implement OnStateExit
        }

        private void OnEnable()
        {
            Transitions = new()
            {
                new Transition("Low Resources", tankAI => tankAI.Health <= 30.0f || tankAI.Ammo <= 4.0f || tankAI.Fuel <= 50.0f),
                new Transition("Tank Found", tankAI => tankAI.EnemyTank)
            };
        }
    }
}
