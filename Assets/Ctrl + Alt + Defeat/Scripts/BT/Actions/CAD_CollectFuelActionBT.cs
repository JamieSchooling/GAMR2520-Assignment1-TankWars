using System.Linq;

public class CAD_CollectFuelActionBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        var fuelConsumables = tankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Fuel"));

        if (fuelConsumables.Count() > 0)
        {
            tankAI.GoTo(fuelConsumables.First().Key.transform.position);
            return CAD_NodeStateBT.Running;
        }
        
        return CAD_NodeStateBT.Success;
    }
}
