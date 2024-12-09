using System;

public class CAD_ConditionNode : CAD_BTNode
{
    private Func<CAD_SmartTankBT, bool> m_Condition;

    public CAD_ConditionNode(Func<CAD_SmartTankBT, bool> condition)
    {
        m_Condition = condition;
    }

    public override CAD_BTState Execute(CAD_SmartTankBT tankAI)
    {
        if (m_Condition(tankAI))
        {
            return CAD_BTState.Success;
        }
        else
        {
            return CAD_BTState.Failure;
        }
    }
}
