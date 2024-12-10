using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateNodeMenu("Behaviour Tree/Selector")]
public class CAD_SelectorNode : CAD_NodeBT
{
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;
    [Output(ShowBackingValue.Never), SerializeField] private int m_Children;

    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        foreach (CAD_NodeBT node in GetConnectedChildren())
        {
            CAD_NodeStateBT state = node.Execute(tankAI);
            if (state == CAD_NodeStateBT.Success)
                return CAD_NodeStateBT.Success;
        }
        return CAD_NodeStateBT.Failure;
    }
}
