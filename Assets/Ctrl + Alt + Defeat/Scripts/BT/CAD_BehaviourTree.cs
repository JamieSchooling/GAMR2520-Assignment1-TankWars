using System.Collections.Generic;

public class CAD_BehaviourTree
{
    private List<CAD_BTNode> m_Nodes = new();

    public List<CAD_BTNode> Nodes => m_Nodes;

    public void AddNode(CAD_BTNode node)
    {
        m_Nodes.Add(node);
    }
}
