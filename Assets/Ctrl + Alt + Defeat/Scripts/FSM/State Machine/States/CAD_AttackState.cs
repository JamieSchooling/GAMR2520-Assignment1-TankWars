using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the state in which the tank will go on the offence and commence attacking the enemy tank.
/// Also handles movement while in aggression/engaged with the enemy.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Attack State")]

public class CAD_AttackState : CAD_State
{
    /// <summary>
    /// Keeps a record of if the tank has been shot by the enemy
    /// </summary>
    private bool m_GotShot = false;
    /// <summary>
    /// Keeps a record of how much health the tank has when it enters aggression
    /// </summary>
    private float m_HealthOnStateEnter;
    /// <summary>
    /// Keeps a record of where the tank should be aiming before firing a shot
    /// </summary>
    private GameObject m_AimSpot;

    /// <summary>
    /// Gets the tanks starting health as we enter the state
    /// </summary>
    /// <param name="tankAI"></param>
    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {
        m_HealthOnStateEnter = tankAI.Health;
    }

    /// <summary>
    /// Gets the enemy position, fires at that point.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance entering the state.</param>
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
        // Destroys the variable to not flood the scene with redundant objects
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