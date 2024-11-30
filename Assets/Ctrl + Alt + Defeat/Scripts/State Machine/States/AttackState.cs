using UnityEngine;

namespace CAD
{
    public class AttackState : IState
    {
        public void OnStateEnter(SmartTank tankAI)
        {
            Debug.Log("Enter Attack State");
            // TODO: Implement OnStateEnter
        }

        public void OnStateUpdate(SmartTank tankAI)
        {
            if (tankAI.EnemyTank) tankAI.TurretFireAtPoint(tankAI.EnemyTank);
        }

        public void OnStateExit(SmartTank tankAI)
        {
            // TODO: Implement OnStateExit
        }
    }
}
