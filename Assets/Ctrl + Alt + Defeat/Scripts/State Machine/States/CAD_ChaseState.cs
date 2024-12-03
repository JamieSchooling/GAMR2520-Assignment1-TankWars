using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the state where the AI-controlled tank chases an enemy tank. The tank follows the enemy tank's position.
/// If the tank loses the enemy or has low resources, it transitions to other states.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Chase State")]
public class CAD_ChaseState : CAD_State
{
    /// <summary>
    /// Stores the last known position of the enemy tank.
    /// </summary>
    private Vector3 m_EnemyPos;

    public override void OnStateEnter(CAD_SmartTank tankAI)
    {
        // TODO: Implement OnStateEnter
    }

    /// <summary>
    ///  Called every frame to update the state behavior. Follows the enemy tank's position.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTank tankAI)
    {
        if (tankAI.EnemyTank)
        {
            tankAI.FollowPathToWorldPoint(tankAI.EnemyTank, 1f);
            m_EnemyPos = tankAI.EnemyTank.transform.position;
        }
    }

    /// <summary>
    /// Called when the state is exited. The last known position of the enemy tank is saved.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance exiting the state.</param>
    public override void OnStateExit(CAD_SmartTank tankAI)
    {
        GameObject lastEnemyPos = new GameObject("LastEnemyPos");
        lastEnemyPos.transform.position = m_EnemyPos;
        tankAI.LastKnownEnemyPos = lastEnemyPos;
    }

    /// <summary>
    /// Creates a list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Low Health or Fuel", tankAI => tankAI.Health <= 30.0f || tankAI.Fuel <= 50.0f),
            new CAD_Transition("Low Ammo", tankAI.Ammo == 0.0f),
            new CAD_Transition("Tank Lost", tankAI => !tankAI.EnemyTank),
            new CAD_Transition("Tank In Range", tankAI => Vector3.Distance(tankAI.transform.position, tankAI.EnemyTank.transform.position) < 25.0f)
        };
    }
}
