using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the state where the AI-controlled tank attacks an enemy tank. The tank fires at the enemy when detected.
/// If the tank loses the enemy or has low resources, it transitions to other states.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Attack State")]
public class CAD_AttackState : CAD_State
{
    private GameObject m_TargetPos;
    private GameObject m_MidPoint;
    private PathStage m_CurrentStage = PathStage.None;

    enum PathStage
    {
        None,
        MidPoint,
        TargetPoint
    }

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
        if (!tankAI.EnemyTank) return;

        Transform enemyTurret = tankAI.EnemyTank.transform.Find("Model/Turret");
        Vector3 direction = tankAI.EnemyTank.transform.position - tankAI.transform.position;
        Vector3 aimOffset = new Vector3(0, 0, 10);

        if (Vector3.Dot(direction.normalized, enemyTurret.forward) > 0 && m_CurrentStage == PathStage.None)
        {
            Vector3 offsetPos = tankAI.EnemyTank.transform.position + aimOffset;
            offsetPos.x *= enemyTurret.forward.x;
            offsetPos.y *= enemyTurret.forward.y;
            offsetPos.z *= enemyTurret.forward.z;

            m_TargetPos = new GameObject("attackPos");
            m_TargetPos.transform.position = offsetPos;

            Vector3 tankToTarget = offsetPos - tankAI.transform.position;

            Debug.DrawLine(tankAI.transform.position, m_TargetPos.transform.position, Color.black);

            Vector3 halfwayPoint = tankAI.transform.position + (tankToTarget * 0.5f) + aimOffset;

            halfwayPoint.x *= enemyTurret.right.x;
            halfwayPoint.y *= enemyTurret.right.y;
            halfwayPoint.z *= enemyTurret.right.z;

            m_MidPoint = new GameObject("midPoint");
            m_MidPoint.transform.position = halfwayPoint;
            Debug.DrawLine(tankAI.transform.position, m_MidPoint.transform.position, Color.green);

            tankAI.FollowPathToWorldPoint(m_MidPoint, 1);
            m_CurrentStage = PathStage.MidPoint;
        }
        else if (Vector3.Dot(direction.normalized, enemyTurret.forward) < 0 && m_CurrentStage == PathStage.None)
        {
            tankAI.TurretFireAtPoint(tankAI.EnemyTank);
        }

        if (m_CurrentStage == PathStage.MidPoint && Vector3.Distance(tankAI.transform.position, m_MidPoint.transform.position) < 1)
        {
            Destroy(m_MidPoint);
            m_MidPoint = null;
            tankAI.FollowPathToWorldPoint(m_TargetPos, 1);
            m_CurrentStage = PathStage.TargetPoint;
        }

        if (m_CurrentStage == PathStage.TargetPoint && Vector3.Distance(tankAI.transform.position, m_MidPoint.transform.position) < 1)
        {
            Destroy(m_TargetPos);
            m_TargetPos = null;
            m_CurrentStage = PathStage.None;
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
            new CAD_Transition("Low Resources", tankAI => tankAI.Health <= 30.0f || tankAI.Ammo <= 4.0f || tankAI.Fuel <= 50.0f),
            new CAD_Transition("Tank Lost", tankAI => !tankAI.EnemyTank)
        };
    }
}
