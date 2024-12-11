using System;
using UnityEditor;
using UnityEngine;

[CreateNodeMenu("Behaviour Tree/Action")]
public class CAD_ActionNode : CAD_NodeBT
{
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;

    [SerializeField] private MonoScript m_Action;

    private CAD_ActionBT m_ActionInstance;

    protected override void Init()
    {
        if (m_Action == null) return;

        m_ActionInstance = CreateInstance(m_Action.GetClass()) as CAD_ActionBT;
    }

    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        return m_ActionInstance.Execute(tankAI);
    }
}
