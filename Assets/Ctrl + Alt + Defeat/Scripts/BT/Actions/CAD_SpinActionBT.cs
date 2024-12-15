using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an action node that makes the AI-controlled tank spin its turret.
/// </summary>
public class CAD_SpinActionBT : CAD_ActionBT
{
    /// <summary>
    /// Executes the action to spin the turret of the AI tank.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this action.</param>
    /// <returns>
    /// Always returns <see cref="CAD_NodeStateBT.Success"/> as the action is instantaneous and does not fail.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Command the tank to spin its turret.
        tankAI.SpinTurret();
        return CAD_NodeStateBT.Success;
    }
}
