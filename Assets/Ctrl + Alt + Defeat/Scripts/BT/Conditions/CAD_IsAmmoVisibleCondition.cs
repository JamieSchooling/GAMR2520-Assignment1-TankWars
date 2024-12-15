using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a condition that checks if ammo consumables are visible to the AI-controlled tank.
/// </summary>
public class CAD_IsAmmoVisibleCondition : CAD_ConditionBT
{
    /// <summary>
    /// Evaluates whether any ammo consumables are currently visible to the AI tank.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this condition.</param>
    /// <returns>
    /// Returns true if there is at least one visible ammo consumable; otherwise, false.
    /// </returns>
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        // Check if any consumables with the "Ammo" tag are visible.
        return tankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Ammo")).Count() > 0;
    }
}
