using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a condition that checks if fuel consumables are visible to the AI-controlled tank.
/// </summary>
public class CAD_IsFuelVisibleCondition : CAD_ConditionBT
{
    /// <summary>
    /// Evaluates whether any fuel consumables are currently visible to the AI tank.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this condition.</param>
    /// <returns>
    /// Returns true if there is at least one visible fuel consumable; otherwise, false.
    /// </returns>
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        // Check if any consumables with the "Fuel" tag are visible.
        return tankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Fuel")).Count() > 0;
    }
}
