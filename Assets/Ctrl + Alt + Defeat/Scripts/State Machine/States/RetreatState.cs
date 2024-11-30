using UnityEngine;
using System.Linq;

namespace CAD
{
    public class RetreatState : IState
    {
        private float m_CurrentTime = 0;

        public void OnStateEnter(SmartTank tankAI)
        {
            Debug.Log("Enter Retreat State");
        }

        public void OnStateUpdate(SmartTank tankAI)
        {
            if (tankAI.Health <= 30.0f)
            {
                FindConsumable(tankAI, "Health");
            }
            if (tankAI.Ammo <= 4.0f)
            {
                FindConsumable(tankAI, "Ammo");
            }
            if (tankAI.Fuel <= 50.0f)
            {
                FindConsumable(tankAI, "Fuel");
            }
        }

        public void OnStateExit(SmartTank tankAI)
        {
            // TODO: Implement OnStateExit
        }

        private void FindConsumable(SmartTank tankAI, string consumableType)
        {
            if (tankAI.VisibleConsumables.Count > 0)
            {
                var potentialConsumables = tankAI.VisibleConsumables.Where(c => c.Key.CompareTag(consumableType)).ToDictionary(i => i.Key, i => i.Value);

                if (potentialConsumables.Count > 0)
                {
                    GameObject consumable = potentialConsumables.First().Key;
                    tankAI.FollowPathToWorldPoint(consumable, 1f);
                    m_CurrentTime += Time.deltaTime;
                }
                else
                {
                    tankAI.FollowPathToRandomWorldPoint(1f);
                }
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
    }
}
