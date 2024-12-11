using UnityEditor;
using UnityEngine;

[CreateNodeMenu("Behaviour Tree/Condition")]
public class CAD_ConditionNode : CAD_NodeBT
{
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;
    [Output(ShowBackingValue.Never), SerializeField] private int m_Children;

    [SerializeField] private MonoScript m_Condition;

    private CAD_ConditionBT m_ConditionInstance;

    protected override void Init()
    {
        m_ConditionInstance = CreateInstance(m_Condition.GetClass()) as CAD_ConditionBT;
    }

    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        if (m_ConditionInstance.Evaluate(tankAI))
        {
            return GetConnectedChildren()[0].Execute(tankAI);
        }
        else
        {
            return CAD_NodeStateBT.Failure;
        }
    }
}
