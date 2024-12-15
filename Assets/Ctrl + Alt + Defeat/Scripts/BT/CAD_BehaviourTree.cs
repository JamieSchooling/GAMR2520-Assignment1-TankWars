using UnityEngine;
using XNode;

/// <summary>
/// Represents a behavior tree for AI decision-making. 
/// The behavior tree is composed of interconnected nodes that define the AI's logic.
/// </summary>
[CreateAssetMenu(menuName = "AI/Behaviour Tree")]
public class CAD_BehaviourTree : NodeGraph
{
    /// <summary>
    /// The root node of the behavior tree. Execution starts from this node.
    /// </summary>
    private CAD_NodeBT m_Root;

    /// <summary>
    /// Gets the root node of the behavior tree.
    /// </summary>
    public CAD_NodeBT Root => m_Root;

    /// <summary>
    /// Initializes the behavior tree by locating the root node.
    /// </summary>
    public void Start()
    {
        // Finds the root node in the node graph, which is expected to be a CAD_RootNode.
        m_Root = nodes.Find(n => n is CAD_RootNode) as CAD_NodeBT;
    }
}
