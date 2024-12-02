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
        if (tankAI.VisibleEnemyBases.Count > 0)
        {
            if (Vector3.Distance(tankAI.transform.position, tankAI.VisibleEnemyBases.First().Key.transform.position) > 25.0f)
            {
                tankAI.FollowPathToWorldPoint(tankAI.VisibleEnemyBases.First().Key, 1f);
            }
            else
            {
                tankAI.TurretFireAtPoint(tankAI.VisibleEnemyBases.First().Key);
            }
        }
    }

    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Low Resources", tankAI => tankAI.Health <= 30.0f || tankAI.Ammo <= 4.0f || tankAI.Fuel <= 50.0f),
            new CAD_Transition("Tank Found", tankAI => tankAI.EnemyTank),
            new CAD_Transition("Base Lost", tankAI => tankAI.VisibleEnemyBases.Count == 0)
        };
    }
}

