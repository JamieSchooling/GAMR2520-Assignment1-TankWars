using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Represents a state where the AI-controlled tank retreats until a safe distance from the assailant.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Retreat State")]
public class CAD_RetreatState : CAD_State
{
    /// <summary>
    /// Holds the time in seconds since the last random path finding target was generated.
    /// </summary>
    private float m_CurrentTime = 0;

    public override void OnStateEnter(CAD_SmartTank tankAI)
    {

    }

    /// <summary>
    ///  Called every frame to update the state behavior. Goes to the opposite side from enemy tank's position.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTank tankAI)
    {
        if (tankAI.EnemyTank)
        {
            tankAI.FollowPathToWorldPoint((tankAI.EnemyTank*-1), 1f);
            m_EnemyPos = tankAI.EnemyTank.transform.position;
        }
    }

    public override void OnStateExit(CAD_SmartTank tankAI)
    {
        GameObject lastEnemyPos = new GameObject("LastEnemyPos");
        lastEnemyPos.transform.position = m_EnemyPos;
        tankAI.LastKnownEnemyPos = lastEnemyPos;
    }

    /// <summary>
    /// Creates list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Transitions = new()
        {

            new CAD_Transition("Safe Distance", tankAI => tankAI.Health > 30 && tankAI.Ammo > 4 && tankAI.Fuel > 50)
        };
    }
}
