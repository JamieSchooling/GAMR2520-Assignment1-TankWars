using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a condition that checks if an enemy base is within range of the AI-controlled tank.
/// </summary>
[CreateAssetMenu(menuName = "AI/BT/Conditions")]
public class CAD_IsBaseInRangeCondition : CAD_ConditionBT
{
    /// <summary>
    /// Evaluates whether the closest visible enemy base is within 20 units of the AI tank.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this condition.</param>
    /// <returns>
    /// Returns true if the closest enemy base is within 20 units; otherwise, false.
    /// </returns>
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        // Ensure there are visible enemy bases before attempting to evaluate distance.
        if (tankAI.BasesFound.Count == 0) return false;

        // Calculate the distance to the closest enemy base.
        float distance = Vector3.Distance(
            tankAI.BasesFound.OrderBy(b => b.Value).First().Key.transform.position,
            tankAI.transform.position
        );

        // Check if the distance is within the range threshold.
        return distance < 20.0f;
    }
}
