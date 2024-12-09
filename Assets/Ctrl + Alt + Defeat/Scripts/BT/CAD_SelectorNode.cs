using System.Collections.Generic;

public class CAD_SelectorNode : CAD_BTNode
{
    private List<CAD_BTNode> m_Children;

    public CAD_SelectorNode(params CAD_SequenceNode[] children)
    {
        m_Children.AddRange(children);
    }

    public override CAD_BTState Execute(CAD_SmartTankBT tankAI)
    {
        foreach (CAD_BTNode node in m_Children)
        {
            CAD_BTState state = node.Execute(tankAI);
            if (state == CAD_BTState.Success)
                return CAD_BTState.Success;
        }
        return CAD_BTState.Failure;
    }
}
