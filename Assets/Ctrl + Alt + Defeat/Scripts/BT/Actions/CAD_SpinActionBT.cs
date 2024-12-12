using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAD_SpinActionBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        tankAI.SpinTurret();
        return CAD_NodeStateBT.Success;
    }
}
