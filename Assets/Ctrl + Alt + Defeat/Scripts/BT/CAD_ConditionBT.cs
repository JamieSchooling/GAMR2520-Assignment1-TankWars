using UnityEngine;

/// <summary>
/// Represents the base class for all conditions in a behavior tree.
/// </summary>
public abstract class CAD_ConditionBT : ScriptableObject
{
    /// <summary>
    /// Evaluates the condition against the current state of the provided tank AI.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance to evaluate the condition on.</param>
    /// <returns>
    /// <c>A boolean indiciating whether the condition is met.</c>.
    /// </returns>
    public abstract bool Evaluate(CAD_SmartTankBT tankAI);
}
