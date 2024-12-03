using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the state where the AI-controlled tank attacks an enemy tank. The tank fires at the enemy when detected.
/// If the tank loses the enemy or has low resources, it transitions to other states.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Attack State")]
public class CAD_AttackState : CAD_State
{
    public override void OnStateEnter(CAD_SmartTank tankAI)
    {
        // TODO: Implement OnStateEnter
    }

    /// <summary>
    /// Called every frame to update the state behavior. Fires at the enemy tank if detected.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTank tankAI)
    {
        if (tankAI.EnemyTank) tankAI.TurretFireAtPoint(tankAI.EnemyTank);
    }

    public override void OnStateExit(CAD_SmartTank tankAI)
    {
        // TODO: Implement OnStateExit
    }

    /// <summary>
    /// Creates a list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Low Health or Fuel", tankAI => tankAI.Health <= 30.0f || tankAI.Fuel <= 50.0f),
            new CAD_Transition("Low Ammo", tankAI => tankAI.Ammo == 0.0f),
            new CAD_Transition("Tank Lost", tankAI => !tankAI.EnemyTank)
        };
    }
}
