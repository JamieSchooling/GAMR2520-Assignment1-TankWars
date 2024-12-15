using System.Linq;

/// <summary>
/// Represents an action node that moves the AI-controlled tank to collect the nearest ammo consumable.
/// </summary>
public class CAD_CollectAmmoBT : CAD_ActionBT
{
    /// <summary>
    /// Executes the action to move the AI tank towards the nearest visible ammo consumable.
    /// If no ammo consumables are visible, it succeeds immediately.
    /// Otherwise, it moves the tank towards the consumable and reports the action as still running.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this action.</param>
    /// <returns>
    /// - <see cref="CAD_NodeStateBT.Running"/> if the tank is moving towards an ammo consumable.
    /// - <see cref="CAD_NodeStateBT.Success"/> if no ammo consumables are visible.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Find all visible ammo consumables.
        var ammoConsumables = tankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Ammo"));

        // If there are any ammo consumables, move towards the nearest one.
        if (ammoConsumables.Count() > 0)
        {
            tankAI.GoTo(ammoConsumables.First().Key.transform.position);
            return CAD_NodeStateBT.Running;
        }

        // If no ammo consumables are visible, the action succeeds.
        return CAD_NodeStateBT.Success;
    }
}
