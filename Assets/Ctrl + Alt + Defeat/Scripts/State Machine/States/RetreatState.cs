using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace CAD
{
    [CreateAssetMenu(menuName = "AI/States/Retreat State")]
    public class RetreatState : State
    {
        private float m_CurrentTime = 0;

        public override void OnStateEnter(SmartTank tankAI)
        {
        }   

        public override void OnStateUpdate(SmartTank tankAI)
        {
            List<string> consumablesToFind = new();

            if (tankAI.Health <= 30.0f)
            {
                consumablesToFind.Add("Health");
            }
            if (tankAI.Ammo <= 4.0f)
            {
                consumablesToFind.Add("Ammo");
            }
            if (tankAI.Fuel <= 50.0f)
            {
                consumablesToFind.Add("Fuel");
            }

            FindConsumables(tankAI, consumablesToFind);
        }

        public override void OnStateExit(SmartTank tankAI)
        {
            // TODO: Implement OnStateExit
        }

        private void FindConsumables(SmartTank tankAI, List<string> consumableTypes)
        {
            if (tankAI.VisibleConsumables.Count > 0)
            {
                var potentialConsumables = tankAI.VisibleConsumables
                    .Where(c => consumableTypes.Any(type => c.Key.CompareTag(type)))
                    .ToDictionary(i => i.Key, i => i.Value);

                if (potentialConsumables.Count > 0)
                {
                    GameObject consumable = potentialConsumables.First().Key;
                    tankAI.FollowPathToWorldPoint(consumable, 1f);
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

        private void OnEnable()
        {
            Transitions = new()
            {
                new Transition("Tank Found", tankAI => tankAI.EnemyTank),
                new Transition("Tank Lost", tankAI => !tankAI.EnemyTank)
            };
        }
    }
}
