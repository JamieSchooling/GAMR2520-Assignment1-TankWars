using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a Selector node in a behavior tree.
/// The Selector node executes its child nodes in order until one succeeds or all fail:
/// - If a child node returns <c>Success</c>, the selector stops and returns <c>Success</c>.
/// - If a child node returns <c>Running</c>, the selector stops and returns <c>Running</c>.
/// - If all child nodes return <c>Failure</c>, the selector returns <c>Failure</c>.
/// </summary>
[CreateNodeMenu("Behaviour Tree/Selector")]
public class CAD_SelectorNode : CAD_NodeBT
{
    /// <summary>
    /// Input port for the parent node. This value is not used directly in logic but represents 
    /// the parent node connection in the graph.
    /// </summary>
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;

    /// <summary>
    /// Output port for the child nodes. This value is not used directly in logic but represents 
    /// the child node connections in the graph.
    /// </summary>
    [Output(ShowBackingValue.Never), SerializeField] private int m_Children;

    /// <summary>
    /// Executes the child nodes in order until one succeeds or all fail.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this behavior tree.</param>
    /// <returns>
    /// A <see cref="CAD_NodeStateBT"/> indicating the result of the selector execution.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        foreach (CAD_NodeBT node in GetConnectedChildren())
        {
            CAD_NodeStateBT state = node.Execute(tankAI);

            // If a child node is still running, return Running.
            if (state == CAD_NodeStateBT.Running)
                return CAD_NodeStateBT.Running;

            // If a child node succeeds, return Success.
            if (state == CAD_NodeStateBT.Success)
                return CAD_NodeStateBT.Success;
        }

        // If all child nodes fail, return Failure.
        return CAD_NodeStateBT.Failure;
    }
}
