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

    /// <summary>
    /// Holds a Positive x offset
    /// </summary>
    Vector3 m_PosXOffset = new Vector3(25, 0, 0);

    /// <summary>
    /// Holds a Negative x offset
    /// </summary>
    Vector3 m_NegXOffset = new Vector3(-25, 0, 0);

    /// <summary>
    /// Holds a Positive z offset
    /// </summary>
    Vector3 m_PosZOffset = new Vector3(0, 0, 25);

    /// <summary>
    /// Holds a Negative z offset
    /// </summary>
    Vector3 m_NegZOffset = new Vector3(0, 0, -25);

    /// <summary>
    /// Holds the current waypint game object
    /// </summary>
    private GameObject m_CurrentWaypoint;

    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {

    }

    /// <summary>
    ///  Called every frame to update the state behavior. Goes to the opposite side from enemy tank's position.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
    {
        GameObject safestPos = new GameObject("SafestPos");
        safestPos.transform.position = m_EnemyPos * -1;
        tankAI.LastKnownSafestPos = safestPos;
        if (tankAI.VisibleEnemyBases.Count > 0)
        {
            if (Vector3.Distance(tankAI.transform.position, tankAI.VisibleEnemyBases.First().Key.transform.position) < 50.0f)
            {
                Vector3 enemyBaseOffset = tankAI.VisibleEnemyBases.First().Key.transform.position + m_PosXOffset + m_PosZOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(enemyBaseOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
        }
        if (tankAI.FriendlyBases.Count > 0)
        {
            var closestFriendlyBase = tankAI.FriendlyBases.OrderBy(b => b.Value).First().Key;
            if (Vector3.Distance(tankAI.transform.position, closestFriendlyBase.transform.position) < 50.0f)
            {
                Vector3 friendlyBaseOffset = closestFriendlyBase.transform.position + m_NegXOffset + m_NegZOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(friendlyBaseOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
        }
        if (tankAI.EnemyTank)
        {
            if (Vector3.Distance(tankAI.transform.position, tankAI.EnemyTank.transform.position) >= 50.0f)
            {
                GoToSafePos(tankAI);
                return;
            }

            if (tankAI.transform.position.x > tankAI.EnemyTank.transform.position.x)
            {
                Vector3 tankOffset = tankAI.EnemyTank.transform.position + m_PosXOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(tankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
            if (tankAI.transform.position.x < tankAI.EnemyTank.transform.position.x)
            {
                Vector3 tankOffset = tankAI.EnemyTank.transform.position + m_NegXOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(tankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
            if (tankAI.transform.position.z > tankAI.EnemyTank.transform.position.z)
            {
                Vector3 tankOffset = tankAI.EnemyTank.transform.position + m_PosZOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(tankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
            if (tankAI.transform.position.z < tankAI.EnemyTank.transform.position.z)
            {
                Vector3 tankOffset = tankAI.EnemyTank.transform.position + m_NegZOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(tankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
        }
        GoToSafePos(tankAI);
    }

    private void GoToSafePos(CAD_SmartTankFSM tankAI)
    {
        tankAI.FollowPathToWorldPoint(tankAI.LastKnownSafestPos, 1f);
        if (tankAI.EnemyTank) m_EnemyPos = tankAI.EnemyTank.transform.position;
    }

    public override void OnStateExit(CAD_SmartTankFSM tankAI)
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
            new CAD_Transition("Safe Distance", tankAI => Vector3.Distance(tankAI.transform.position, tankAI.LastKnownSafestPos.transform.position) < 25.0f),
            new CAD_Transition("Fuel too Low", tankAI => tankAI.Fuel <= 30.0f),
            new CAD_Transition("Fuel Spotted", tankAI => tankAI.VisibleConsumables.Where(c => c.Key.CompareTag("Fuel")).Count() > 0)
        };
    }
}
