using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Sequence node in a behavior tree.
/// The Sequence node executes its child nodes in order until one fails or all succeed:
/// - If a child node returns <c>Failure</c>, the sequence stops and returns <c>Failure</c>.
/// - If a child node returns <c>Running</c>, the sequence stops and returns <c>Running</c>.
/// - If all child nodes return <c>Success</c>, the sequence returns <c>Success</c>.
/// </summary>
[CreateNodeMenu("Behaviour Tree/Sequence")]
public class CAD_SequenceNode : CAD_NodeBT
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
    /// Executes the child nodes in sequence.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this behavior tree.</param>
    /// <returns>
    /// A <see cref="CAD_NodeStateBT"/> indicating the result of the sequence execution.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        foreach (CAD_NodeBT node in GetConnectedChildren())
        {
            CAD_NodeStateBT state = node.Execute(tankAI);

            // If the child node is still running, return Running.
            if (state == CAD_NodeStateBT.Running)
                return CAD_NodeStateBT.Running;

            // If the child node fails, return Failure.
            if (state == CAD_NodeStateBT.Failure)
                return CAD_NodeStateBT.Failure;
        }

        // If all child nodes succeed, return Success.
        return CAD_NodeStateBT.Success;
    }
}
