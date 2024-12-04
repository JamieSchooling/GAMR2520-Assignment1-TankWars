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
    Vector3 XPOffset = new Vector3(25, 0, 0);
    /// <summary>
    /// Holds a Negative x offset
    /// </summary>
    Vector3 XNOffset = new Vector3(-25, 0, 0);
    /// <summary>
    /// Holds a Positive z offset
    /// </summary>
    Vector3 ZPOffset = new Vector3(0, 0, 25);
    /// <summary>
    /// Holds a Negative z offset
    /// </summary>
    Vector3 ZNOffset = new Vector3(0, 0, -25);

    /// <summary>
    /// Holds the current waypint game object
    /// </summary>
    private GameObject m_CurrentWaypoint;

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
        if (tankAI.VisibleEnemyBases.Count > 0)
        {
            if (Vector3.Distance(tankAI.transform.position, tankAI.VisibleEnemyBases.First().Key.transform.position) < 50.0f)
            {
                Vector3 EBaseOffset = tankAI.VisibleEnemyBases.First().Key.transform.position + XPOffset + ZPOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(EBaseOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
        }
        if (tankAI.FriendlyBases.Count > 0)
        {
            var closestfbase = tankAI.FriendlyBases.OrderBy(b => b.Value).First().Key;
            if (Vector3.Distance(tankAI.transform.position, closestfbase.transform.position) < 50.0f)
            {
                Vector3 FBaseOffset = closestfbase.transform.position + XNOffset + ZNOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(FBaseOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
        }
        if (tankAI.EnemyTank)
        {
            if (Vector3.Distance(tankAI.transform.position, tankAI.EnemyTank.transform.position) >= 50.0f)
            {
                PathtoSafe(tankAI);
                return;
            }

            if (tankAI.transform.position.x > tankAI.EnemyTank.transform.position.x)
            {
                Vector3 TankOffset = tankAI.EnemyTank.transform.position + XPOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(TankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
            if (tankAI.transform.position.x < tankAI.EnemyTank.transform.position.x)
            {
                Vector3 TankOffset = tankAI.EnemyTank.transform.position + XNOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(TankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
            if (tankAI.transform.position.z > tankAI.EnemyTank.transform.position.z)
            {
                Vector3 TankOffset = tankAI.EnemyTank.transform.position + ZPOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(TankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
            if (tankAI.transform.position.z < tankAI.EnemyTank.transform.position.z)
            {
                Vector3 TankOffset = tankAI.EnemyTank.transform.position + ZNOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(TankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
        }
        PathtoSafe(tankAI);
    }

    private void PathtoSafe(CAD_SmartTank tankAI)
    {
        tankAI.FollowPathToWorldPoint(tankAI.LastKnownSafestPos, 1f);
        if (tankAI.EnemyTank) m_EnemyPos = tankAI.EnemyTank.transform.position;
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

            new CAD_Transition("Safe Distance", tankAI => Vector3.Distance(tankAI.transform.position, tankAI.LastKnownSafestPos.transform.position) < 25.0f),
            new CAD_Transition("Fuel too Low", tankAI => tankAI.Fuel <= 30.0f),
            new CAD_Transition("Fuel Spotted", tankAI => tankAI.VisibleConsumables.Where(c => c.Key.CompareTag("Fuel")).Count() > 0)
        };
    }
}
