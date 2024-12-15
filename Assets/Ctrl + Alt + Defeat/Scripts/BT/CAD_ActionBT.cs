using UnityEngine;

/// <summary>
/// Represents the base class for all actions in a behavior tree.
/// Actions define specific behaviors that an AI can perform when executed.
/// </summary>
public abstract class CAD_ActionBT : ScriptableObject
{
    /// <summary>
    /// Executes the action for the given AI tank instance.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this action.</param>
    /// <returns>
    /// A <see cref="CAD_NodeStateBT"/> indicating the result of the action's execution:
    /// - <c>Success</c> if the action was completed successfully.
    /// - <c>Failure</c> if the action could not be completed.
    /// - <c>Running</c> if the action is still in progress.
    /// </returns>
    public abstract CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI);
}
