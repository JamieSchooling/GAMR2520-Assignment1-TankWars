using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a base class for all behavior tree nodes used in the AI system.
/// </summary>
public abstract class CAD_NodeBT : XNode.Node
{
    /// <summary>
    /// Retrieves all child nodes connected to this node's "m_Children" output port.
    /// The child nodes are sorted by their vertical position in the node editor graph.
    /// </summary>
    /// <returns>
    /// An ordered list of all child nodes. Returns null if the node does not have a "m_Children" port.
    /// </returns>
    protected List<CAD_NodeBT> GetConnectedChildren()
    {
        // Check if the "m_Children" port exists.
        if (!HasPort("m_Children"))
        {
            Debug.LogError("Can't retrieve connected nodes. Node is a leaf node.");
            return null;
        }

        // Collect all connected child nodes from the "m_Children" port.
        List<CAD_NodeBT> nodes = new();
        foreach (var port in GetOutputPort("m_Children").GetConnections())
        {
            nodes.Add(port.node as CAD_NodeBT);
        }

        // Sort the connected nodes by their vertical position in the graph.
        return nodes.OrderBy(n => n.position.y).ToList();
    }

    /// <summary>
    /// Executes the behavior defined by this node. The result of the execution determines
    /// how the behavior tree will proceed.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance that owns this behavior tree.</param>
    /// <returns>
    /// A <see cref="CAD_NodeStateBT"/> indicating the result of the node's execution.
    /// </returns>
    public abstract CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI);
}

/// <summary>
/// Represents the possible states returned by a behavior tree node after execution.
/// </summary>
public enum CAD_NodeStateBT
{
    /// <summary>
    /// Indicates that the node executed successfully.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the node execution failed.
    /// </summary>
    Failure,

    /// <summary>
    /// Indicates that the node execution is still in progress.
    /// </summary>
    Running
}
