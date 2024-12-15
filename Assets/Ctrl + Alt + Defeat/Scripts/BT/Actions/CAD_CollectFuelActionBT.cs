using System.Linq;

/// <summary>
/// Represents an action node that moves the AI-controlled tank to collect the nearest fuel consumable.
/// </summary>
public class CAD_CollectFuelActionBT : CAD_ActionBT
{
    /// <summary>
    /// Executes the action to move the AI tank towards the nearest visible fuel consumable.
    /// If no fuel consumables are visible, it succeeds immediately.
    /// Otherwise, it moves the tank towards the consumable and reports the action as still running.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this action.</param>
    /// <returns>
    /// - <see cref="CAD_NodeStateBT.Running"/> if the tank is moving towards a fuel consumable.
    /// - <see cref="CAD_NodeStateBT.Success"/> if no fuel consumables are visible.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Find all visible fuel consumables.
        var fuelConsumables = tankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Fuel"));

        // If there are any fuel consumables, move towards the nearest one.
        if (fuelConsumables.Count() > 0)
        {
            tankAI.GoTo(fuelConsumables.First().Key.transform.position);
            return CAD_NodeStateBT.Running;
        }

        // If no fuel consumables are visible, the action succeeds.
        return CAD_NodeStateBT.Success;
    }
}
