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
    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {
        
    }

    public override void OnStateExit(CAD_SmartTankFSM tankAI)
    {
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// Checks distance from the enemy bases, moves towards and shoots the closest enemy base.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance entering the state.</param>
    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
    {
        //If the tank cannot see any enemy bases, do nothing
        if (tankAI.VisibleEnemyBases.Count <= 0) return;

        //Checks if the GameObject is Null
        //Should never happen but just in case
        if (!tankAI.VisibleEnemyBases.First().Key) return;

        //Checks if we are too far from the enemy bases
        if (Vector3.Distance(tankAI.transform.position, tankAI.VisibleEnemyBases.First().Key.transform.position) > 25.0f)
        {
            //Moves towards the closest enemy base
            tankAI.FollowPathToWorldPoint(tankAI.VisibleEnemyBases.First().Key, 1f);
        }
        else
        {
            //Shoots the closest enemy base
            tankAI.TurretFireAtPoint(tankAI.VisibleEnemyBases.First().Key);
        }
    }

    /// <summary>
    /// Creates a list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// </summary>
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

