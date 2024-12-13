using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Behaviour Tree/Inverter")]
public class CAD_InverterNodeBT : CAD_NodeBT
{
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;
    [Output(ShowBackingValue.Never), SerializeField] private int m_Children;

    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        CAD_NodeStateBT state = GetConnectedChildren()[0].Execute(tankAI);

        switch (state)
        {
            case CAD_NodeStateBT.Running:
                return CAD_NodeStateBT.Running;
            case CAD_NodeStateBT.Success:
                return CAD_NodeStateBT.Failure;
            case CAD_NodeStateBT.Failure:
                return CAD_NodeStateBT.Success;
            // Default just in case
            default:
                return CAD_NodeStateBT.Failure;
        }
    }
}
