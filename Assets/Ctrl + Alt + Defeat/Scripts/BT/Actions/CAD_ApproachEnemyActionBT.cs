using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CAD_ApproachEnemyActionBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        tankAI.GoTo(tankAI.TanksFound.First().Key.transform.position);

        return CAD_NodeStateBT.Success;
    }
}
