using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When it has gotten an entry shot on the enemy tank, and resources are still healthy it will enter a berserk state that will kite around the enemy while attacking.
/// This allows it to go full on with the offense while we are in safe position to do so.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Kiting State")]

public class CAD_KitingState : CAD_State
{
    private GameObject m_ReposPoint;

    public override void OnStateEnter(CAD_SmartTank tankAI)
    {
        // TODO: Implement OnStateEnter
    }

    public override void OnStateUpdate(CAD_SmartTank tankAI)
    {
        if (!tankAI.EnemyTank) return;

        Transform enemyTurret = tankAI.EnemyTank.transform.Find("Model/Turret");
        Vector3 direction = tankAI.EnemyTank.transform.position - tankAI.transform.position;
        Vector3 kitingGap = direction / 2;

        m_ReposPoint = new GameObject("reposPoint");
        m_ReposPoint.transform.position = kitingGap;

        if (Vector3.Dot(direction.normalized, enemyTurret.forward) < 0)
        {
            tankAI.FollowPathToWorldPoint(m_ReposPoint, 1);
            tankAI.TurretFireAtPoint(tankAI.EnemyTank);
        }
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