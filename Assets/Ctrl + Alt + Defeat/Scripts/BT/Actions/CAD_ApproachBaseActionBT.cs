using System.Linq;

public class CAD_ApproachBaseActionBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        tankAI.GoTo(tankAI.BasesFound.OrderBy(b => b.Value).First().Key.transform.position);

        return CAD_NodeStateBT.Success;
    }
}
