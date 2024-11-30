using System.Linq;
using UnityEngine;

namespace CAD
{
    public class SearchState : IState
    {
        private float m_CurrentTime;

        public void OnStateEnter(SmartTank tankAI)
        {
            Debug.Log("Enter Search State");
            m_CurrentTime = 0;
        }

        public void OnStateUpdate(SmartTank tankAI)
        {
            if (tankAI.VisibleConsumables.Count > 0)
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

        public void OnStateExit(SmartTank tankAI)
        {
            // TODO: Implement OnStateExit
        }
    }
}
