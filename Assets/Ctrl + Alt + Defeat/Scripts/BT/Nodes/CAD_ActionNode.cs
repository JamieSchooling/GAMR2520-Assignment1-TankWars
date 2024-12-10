using System;
using UnityEngine;

[CreateNodeMenu("Behaviour Tree/Action")]
public class CAD_ActionNode : CAD_NodeBT
{
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;

    [SerializeField] private CAD_ActionBT m_Action;

    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        return m_Action.Execute(tankAI);
    }
}
