using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a condition that checks if enemy tanks are visible to the AI-controlled tank.
/// </summary>
public class CAD_IsEnemyVisibleCondition : CAD_ConditionBT
{
    /// <summary>
    /// Evaluates whether any enemy tanks are currently visible to the AI tank.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this condition.</param>
    /// <returns>
    /// Returns true if there is at least one visible enemy tank; otherwise, false.
    /// </returns>
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        return tankAI.TanksFound.Count > 0;
    }
}
