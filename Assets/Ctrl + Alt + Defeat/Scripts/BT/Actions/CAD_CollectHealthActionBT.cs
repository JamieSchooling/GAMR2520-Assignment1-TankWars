using System.Linq;

public class CAD_CollectHealthActionBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        var healthConsumables = tankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Health"));

        if (healthConsumables.Count() > 0)
        {
            tankAI.GoTo(healthConsumables.First().Key.transform.position);
            return CAD_NodeStateBT.Running;
        }
        
        return CAD_NodeStateBT.Success;
    }
}
