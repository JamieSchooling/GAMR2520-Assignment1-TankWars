using System.Linq;

/// <summary>
/// Represents an action node that moves the AI-controlled tank to collect the nearest health consumable.
/// </summary>
public class CAD_CollectHealthActionBT : CAD_ActionBT
{
    /// <summary>
    /// Executes the action to move the AI tank towards the nearest visible health consumable.
    /// If no health consumables are visible, it succeeds immediately.
    /// Otherwise, it moves the tank towards the consumable and reports the action as still running.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this action.</param>
    /// <returns>
    /// - <see cref="CAD_NodeStateBT.Running"/> if the tank is moving towards a health consumable.
    /// - <see cref="CAD_NodeStateBT.Success"/> if no health consumables are visible.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Find all visible health consumables.
        var healthConsumables = tankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Health"));

        // If there are any health consumables, move towards the nearest one.
        if (healthConsumables.Count() > 0)
        {
            tankAI.GoTo(healthConsumables.First().Key.transform.position);
            return CAD_NodeStateBT.Running;
        }

        // If no health consumables are visible, the action succeeds.
        return CAD_NodeStateBT.Success;
    }
}
