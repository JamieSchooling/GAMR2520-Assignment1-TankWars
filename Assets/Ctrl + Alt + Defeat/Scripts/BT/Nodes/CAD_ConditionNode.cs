using System;

public class CAD_ConditionNode : CAD_BTNode
{
    private Func<CAD_SmartTankBT, bool> m_Condition;
    private CAD_BTNode m_Child;

    public CAD_ConditionNode(Func<CAD_SmartTankBT, bool> condition, CAD_BTNode child)
    {
        m_Condition = condition;
        m_Child = child;
    }

    public override CAD_BTState Execute(CAD_SmartTankBT tankAI)
    {
        if (m_Condition(tankAI))
        {
            return m_Child.Execute(tankAI);
        }
        else
        {
            return CAD_BTState.Failure;
        }
    }
}
