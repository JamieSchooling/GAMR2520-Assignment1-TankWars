using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Represents the state where the AI-controlled tank attacks an enemy base. The tank fires at the base when detected.
/// If the tank has low resources, loses/destroys the base or sees an enemy tank, it transitions to other states.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Base Attack State")]

public class CAD_AttackBaseState : CAD_State
{
    public override void OnStateEnter(CAD_SmartTank tankAI)
    {
        
    }

    public override void OnStateExit(CAD_SmartTank tankAI)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(CAD_SmartTank tankAI)
    {
        if (tankAI.VisibleEnemyBases.Count <= 0) return;

        if (!tankAI.VisibleEnemyBases.First().Key) return;

        if (Vector3.Distance(tankAI.transform.position, tankAI.VisibleEnemyBases.First().Key.transform.position) > 25.0f)
        {
            tankAI.FollowPathToWorldPoint(tankAI.VisibleEnemyBases.First().Key, 1f);
        }
        else
        {
            tankAI.TurretFireAtPoint(tankAI.VisibleEnemyBases.First().Key);
        }
    }
    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Low Health or Fuel", tankAI => tankAI.Health <= 20.0f || tankAI.Fuel <= 50.0f),
            new CAD_Transition("Low Ammo", tankAI => tankAI.Ammo == 0.0f),
            new CAD_Transition("Tank Found", tankAI => tankAI.EnemyTank),
            new CAD_Transition("Base Lost", tankAI => tankAI.VisibleEnemyBases.Count == 0)
        };
    }
}

