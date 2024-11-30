using UnityEngine;
using System.Linq;

namespace CAD
{
    public class RetreatState : IState
    {
        private float m_CurrentTime;

        public void OnStateEnter(SmartTank tankAI)
        {
            m_CurrentTime = 0.0f;
        }

        public void OnStateUpdate(SmartTank tankAI)
        {
            if (tankAI.VisibleConsumables.Count > 0)
            {
                GameObject consumable = tankAI.VisibleConsumables.First().Key;
                tankAI.FollowPathToWorldPoint(consumable, 1f);
                m_CurrentTime += Time.deltaTime;
                if (m_CurrentTime > 10)
                {
                    tankAI.GenerateNewRandomWorldPoint();
                    m_CurrentTime = 0;
                }
            }
            else
            {
                tankAI.FollowPathToRandomWorldPoint(1f);
            }
        }

        public void OnStateExit(SmartTank tankAI)
        {
            throw new System.NotImplementedException();
        }
    }
}
