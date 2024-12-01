using System.Collections.Generic;
using UnityEngine;

namespace CAD
{
    [CreateAssetMenu(menuName = "AI/States/Attack State")]
    public class AttackState : State
    {
        public override void OnStateEnter(SmartTank tankAI)
        {
            // TODO: Implement OnStateEnter
        }

        public override void OnStateUpdate(SmartTank tankAI)
        {
            if (tankAI.EnemyTank) tankAI.TurretFireAtPoint(tankAI.EnemyTank);
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
                new Transition("Tank Lost", tankAI => !tankAI.EnemyTank)
            };
        }
    }
}
