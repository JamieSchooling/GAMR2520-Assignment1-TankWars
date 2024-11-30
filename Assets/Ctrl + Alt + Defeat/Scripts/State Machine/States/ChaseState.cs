using UnityEngine;

namespace CAD
{
    public class ChaseState : IState
    {
        public void OnStateEnter(SmartTank tankAI)
        {
            Debug.Log("Enter Chase State");
            // TODO: Implement OnStateEnter
        }

        public void OnStateUpdate(SmartTank tankAI)
        {
            if (tankAI.EnemyTank) tankAI.FollowPathToWorldPoint(tankAI.EnemyTank, 1f);
        }

        public void OnStateExit(SmartTank tankAI)
        {
            // TODO: Implement OnStateExit
        }
    }
}
