using System.Linq;

public class CAD_CollectAmmoBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        var ammoConsumables = tankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Ammo"));

        if (ammoConsumables.Count() > 0)
        {
            tankAI.GoTo(ammoConsumables.First().Key.transform.position);
            return CAD_NodeStateBT.Running;
        }
        
        return CAD_NodeStateBT.Success;
    }
}
