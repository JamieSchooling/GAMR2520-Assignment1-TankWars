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
                // Filter consumables matching the required types
                var potentialConsumables = tankAI.VisibleConsumables
                    .Where(c => consumableTypes.Any(type => c.Key.CompareTag(type)))
                    .OrderBy(c => c.Value) // Order by distance
                    .ToList();

                if (potentialConsumables.Count > 0)
                {
                    // Get the closest consumable
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
                new Transition("Enough Resources", tankAI => tankAI.Health > 30 && tankAI.Ammo > 4 && tankAI.Fuel > 50)
            };
        }
    }
}
