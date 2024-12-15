using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents the root node of a behavior tree. 
/// The root node serves as the starting point for the behavior tree's execution.
/// It can only have one child node connected to it.
/// </summary>
[CreateNodeMenu("Behaviour Tree/Root")]
[DisallowMultipleNodes, NodeTint("#000000")]
public class CAD_RootNode : CAD_NodeBT
{
    /// <summary>
    /// Output port for the child node. This value is not used directly in logic but represents 
    /// the child node connection in the graph.
    /// </summary>
    [Output(ShowBackingValue.Never, ConnectionType.Override), SerializeField] private int m_Children;

    /// <summary>
    /// Executes the single connected child node.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this behavior tree.</param>
    /// <returns>
    /// A <see cref="CAD_NodeStateBT"/> indicating the result of the child node's execution.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Execute the first (and only) connected child node.
        return GetConnectedChildren().First().Execute(tankAI);
    }
}
