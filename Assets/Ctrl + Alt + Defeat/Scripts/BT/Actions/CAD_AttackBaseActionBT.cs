using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents an action node that moves the AI-controlled tank to attack the nearest visible enemy base.
/// </summary>
[CreateAssetMenu(menuName = "AI/BT/Actions")]
public class CAD_AttackBaseActionBT : CAD_ActionBT
{
    /// <summary>
    /// Executes the action to attack the nearest visible enemy base.
    /// If an enemy base is found, the tank fires at its position.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this action.</param>
    /// <returns>
    /// Always returns <see cref="CAD_NodeStateBT.Success"/> as the attack action is considered complete once executed.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Retrieve the position of the nearest enemy base.
        Vector3 position = tankAI.BasesFound.OrderBy(b => b.Value).First().Key.transform.position;

        // Move the tank to attack the base at the specified position.
        tankAI.Attack(position);

        return CAD_NodeStateBT.Success;
    }
}
