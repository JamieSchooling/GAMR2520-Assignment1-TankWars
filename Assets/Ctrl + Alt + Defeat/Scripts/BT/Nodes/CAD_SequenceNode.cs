using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Behaviour Tree/Sequence")]
public class CAD_SequenceNode : CAD_NodeBT
{
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;
    [Output(ShowBackingValue.Never), SerializeField] private int m_Children;

    private int m_CurrentChildIndex;

    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        foreach (CAD_NodeBT node in GetConnectedChildren())
        {
            CAD_NodeStateBT state = node.Execute(tankAI);
            if (state == CAD_NodeStateBT.Running)
                return CAD_NodeStateBT.Running;
            if (state == CAD_NodeStateBT.Failure)
                return CAD_NodeStateBT.Failure;
        }
        return CAD_NodeStateBT.Success;
    }
}
