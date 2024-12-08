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

        Vector3 test = tankAI.transform.position + new Vector3(0, 0, 25);

        Vector3 kitingCorner1 = tankAI.EnemyTank.transform.position + new Vector3(25, 0, -25); // Bottom Right Corner
        Vector3 kitingCorner2 = tankAI.EnemyTank.transform.position + new Vector3(-25, 0, -25); // Bottom Left Corner
        Vector3 kitingCorner3 = tankAI.EnemyTank.transform.position + new Vector3(-25, 0, 25); // Top Left Corner
        Vector3 kitingCorner4 = tankAI.EnemyTank.transform.position + new Vector3(25, 0, 25); // Top Right Corner

        m_ReposPoint = tankAI.CreateWaypoint(test);
    }

    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
    {
        if (!tankAI.EnemyTank) return;

        Transform enemyTurret = tankAI.EnemyTank.transform.Find("Model/Turret");
        Vector3 direction = tankAI.EnemyTank.transform.position - tankAI.transform.position;

        // tankAI.FaceTurretAtPoint(tankAI.EnemyTank); Method doesn't exist anymore?
        //tankAI.FollowPathToWorldPoint(m_ReposPoint, 1);

        //for (int i = 0; i < m_kitingWaypoints.Length; i++)
        //{
        //    float bestStart = float.PositiveInfinity;
        //    float currentStart = Vector3.Distance(tankAI.transform.position, m_kitingWaypoints[i]);
        //    if (currentStart < bestStart)
        //    {
        //        WaypointIndex = i;
        //        bestStart = currentStart;
        //    }
        //    m_CurrentWaypoint = tankAI.CreateWaypoint(m_kitingWaypoints[WaypointIndex]);
        //}

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