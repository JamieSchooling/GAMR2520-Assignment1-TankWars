using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a condition that checks if an enemy tank is within range of the AI-controlled tank.
/// </summary>
[CreateAssetMenu(menuName = "AI/BT/Conditions")]
public class CAD_IsEnemyInRangeCondition : CAD_ConditionBT
{
    /// <summary>
    /// Evaluates whether the closest visible enemy tank is within 25 units of the AI tank.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this condition.</param>
    /// <returns>
    /// Returns true if the closest enemy tank is within 25 units; otherwise, false.
    /// </returns>
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        // Ensure there are visible enemy tanks before attempting to evaluate distance.
        if (tankAI.TanksFound.Count == 0) return false;

        // Calculate the distance to the closest enemy tank.
        float distance = Vector3.Distance(
            tankAI.TanksFound.First().Key.transform.position,
            tankAI.transform.position
        );

        // Check if the distance is within the range threshold.
        return distance < 25.0f;
    }
}
