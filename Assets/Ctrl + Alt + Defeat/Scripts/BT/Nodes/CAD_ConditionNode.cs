using UnityEngine;

[CreateNodeMenu("Behaviour Tree/Condition")]
public class CAD_ConditionNode : CAD_NodeBT
{
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;

    [SerializeField] private CAD_ConditionBT m_Condition;

    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        if (m_Condition.Evaluate(tankAI))
        {
            return CAD_NodeStateBT.Success;
        }
        else
        {
            return CAD_NodeStateBT.Failure;
        }
    }
}
