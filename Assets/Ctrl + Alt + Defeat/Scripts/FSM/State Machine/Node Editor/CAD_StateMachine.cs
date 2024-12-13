using UnityEngine;
using XNode;

/// <summary>
/// A scriptable object that holds all state machine nodes.
/// </summary>
[CreateAssetMenu(menuName = "AI/State Machine")]
public class CAD_StateMachine : NodeGraph
{
    /// <summary>
    /// The current state's node within the graph.
    /// </summary>
    public XNode.Node CurrentNode { get; set; }
}