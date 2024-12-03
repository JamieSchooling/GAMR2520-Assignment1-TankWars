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
    /// Stores the last known position of the enemy tank.
    /// </summary>
    private Vector3 m_EnemyPos;
    private GameObject m_NewPos;
    Vector3 Offset = new Vector3(10, 0, 10);

    public override void OnStateEnter(CAD_SmartTank tankAI)
    {

    }

    /// <summary>
    ///  Called every frame to update the state behavior. Goes to the opposite side from enemy tank's position.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTank tankAI)
    {
        GameObject SafestPos = new GameObject("SafestPos");
        SafestPos.transform.position = m_EnemyPos * -1;
        tankAI.LastKnownSafestPos = SafestPos;
        if (tankAI.EnemyTank)
        {
            Vector3 TankOffset = tankAI.EnemyTank.transform.position + Offset;
            m_NewPos = new GameObject("MoveAround");
            m_NewPos.transform.position = TankOffset;
            tankAI.FollowPathToWorldPoint(m_NewPos, 1f);
            Destroy(m_NewPos);
        }
        tankAI.FollowPathToWorldPoint(tankAI.LastKnownSafestPos, 1f);
        m_EnemyPos = tankAI.EnemyTank.transform.position;
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

            new CAD_Transition("Safe Distance", tankAI => tankAI.transform.position.x <= tankAI.LastKnownSafestPos.transform.position.x - 10.0f && tankAI.transform.position.x >= tankAI.LastKnownSafestPos.transform.position.x + 10.0f
            && tankAI.transform.position.z <= tankAI.LastKnownSafestPos.transform.position.z - 10.0f && tankAI.transform.position.z >= tankAI.LastKnownSafestPos.transform.position.z + 10.0f),
            new CAD_Transition("Fuel too Low", tankAI => tankAI.Fuel <= 30.0f)
        };
    }
}
