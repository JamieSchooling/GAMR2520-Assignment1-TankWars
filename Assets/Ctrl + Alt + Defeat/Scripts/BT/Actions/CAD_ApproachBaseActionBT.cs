using System.Linq;

/// <summary>
/// Represents an action node that moves the AI-controlled tank to approach the nearest visible enemy base.
/// </summary>
public class CAD_ApproachBaseActionBT : CAD_ActionBT
{
    /// <summary>
    /// Executes the action to move the AI tank towards the nearest visible enemy base.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this action.</param>
    /// <returns>
    /// Always returns <see cref="CAD_NodeStateBT.Success"/> as the approach action is considered complete once the movement is initiated.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Move the tank to the position of the nearest enemy base.
        tankAI.GoTo(tankAI.BasesFound.OrderBy(b => b.Value).First().Key.transform.position);

        return CAD_NodeStateBT.Success;
    }
}
