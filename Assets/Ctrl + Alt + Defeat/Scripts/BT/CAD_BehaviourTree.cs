using UnityEngine;
using XNode;

[CreateAssetMenu(menuName = "AI/Behaviour Tree")]
public class CAD_BehaviourTree : NodeGraph
{
    private CAD_NodeBT m_Root;

    public CAD_NodeBT Root => m_Root;

    public void Start()
    {
        m_Root = nodes.Find(n => n is CAD_RootNode) as CAD_NodeBT;
    }
}
