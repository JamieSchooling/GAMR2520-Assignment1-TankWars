using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a condition that checks if the AI-controlled tank has low fuel.
/// </summary>
public class CAD_IsLowFuelCondition : CAD_ConditionBT
{
    /// <summary>
    /// Evaluates whether the tank's fuel level is low (less than or equal to 30).
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this condition.</param>
    /// <returns>
    /// Returns true if the tank's fuel level is less than or equal to 30; otherwise, false.
    /// </returns>
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        return tankAI.FuelLevel <= 30.0f;
    }
}
