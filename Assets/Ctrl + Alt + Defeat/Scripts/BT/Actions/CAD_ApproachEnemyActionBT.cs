using System.Linq;

public class CAD_ApproachEnemyActionBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        tankAI.GoTo(tankAI.TanksFound.First().Key.transform.position);

        return CAD_NodeStateBT.Success;
    }
}
