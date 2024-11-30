using UnityEngine;

namespace CAD
{
    public class SearchState : IState
    {
        private float m_CurrentTime;

        public void OnStateEnter(SmartTank tankAI)
        {
            m_CurrentTime = 0;
        }

        public void OnStateUpdate(SmartTank tankAI)
        {
            tankAI.FollowPathToRandomWorldPoint(1f);
            m_CurrentTime += Time.deltaTime;
            if (m_CurrentTime > 10)
            {
                tankAI.GenerateNewRandomWorldPoint();
                m_CurrentTime = 0;
            }
        }

        public void OnStateExit(SmartTank tankAI)
        {
            throw new System.NotImplementedException();
        }
    }
}
