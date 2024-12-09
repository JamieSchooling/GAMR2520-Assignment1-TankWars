using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When it has gotten an entry shot on the enemy tank, and resources are still healthy it will enter a berserk state that will kite around the enemy while attacking.
/// This allows it to go full on with the offense while we are in safe position to do so.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Attack State")]

public class CAD_AttackState : CAD_State
{
    private GameObject m_ReposPoint;
    [SerializeField] private Vector3[] m_kitingWaypoints;
    private int WaypointIndex = 0;
    private GameObject m_CurrentWaypoint;
    private bool m_GotShot = false;
    private float m_HealthOnStateEnter;
    private GameObject m_AimSpot;

    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {
        m_HealthOnStateEnter = tankAI.Health;
    }

    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
    {
        if (!tankAI.EnemyTank) return;

        Transform enemyLocation = tankAI.EnemyTank.transform;
        Vector3 aimSpot = enemyLocation.position;
        m_AimSpot = tankAI.CreateWaypoint(aimSpot);
        tankAI.TurretFireAtPoint(m_AimSpot);
        Destroy(m_AimSpot);

        if (tankAI.Health < m_HealthOnStateEnter)
        {
            m_GotShot = true;
        }
    }

    public override void OnStateExit(CAD_SmartTankFSM tankAI)
    {
        m_GotShot = false;
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
            new CAD_Transition("Tank Lost", tankAI => !tankAI.EnemyTank),
            new CAD_Transition("Got Shot", tankAI => m_GotShot)

        };
    }
}