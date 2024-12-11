using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Attack State")]

public class CAD_AttackState : CAD_State
{
    //
    private GameObject m_ReposPoint;
    [SerializeField] private Vector3[] m_kitingWaypoints;
    private int WaypointIndex = 0;
    private GameObject m_CurrentWaypoint;
    private bool m_GotShot = false;
    private float m_HealthOnStateEnter;
    private GameObject m_AimSpot;

    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {
        //Gets the tanks starting health as we enter the state
        m_HealthOnStateEnter = tankAI.Health;
    }

    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
    {
        if (!tankAI.EnemyTank) return;

        //Get the enemy position
        Transform enemyLocation = tankAI.EnemyTank.transform;
        //Create a vector for the point to aim
        Vector3 aimSpot = enemyLocation.position;
        //Fire at the waypoint
        m_AimSpot = tankAI.CreateWaypoint(aimSpot);
        tankAI.TurretFireAtPoint(m_AimSpot);
        Destroy(m_AimSpot);

        //Registers when we get shot
        if (tankAI.Health < m_HealthOnStateEnter)
        {
            m_GotShot = true;
        }
    }

    public override void OnStateExit(CAD_SmartTankFSM tankAI)
    {
        //Resets if we got shot on state exit
        m_GotShot = false;
    }

    /// <summary>
    /// Creates a list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// Should the tank's health or fuel be too low we move to the retreat state
    /// Should the tank's ammo be too low we move to the resource gathering state
    /// Should the tank lose the enemy tank we move to the search state
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