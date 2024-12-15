using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an inverter node in a behavior tree. 
/// The inverter node inverts the result of its single child node:
/// - Success becomes Failure
/// - Failure becomes Success
/// - Running remains Running
/// </summary>
[CreateNodeMenu("Behaviour Tree/Inverter")]
public class CAD_InverterNodeBT : CAD_NodeBT
{
    /// <summary>
    /// Input port for the parent node. This value is not used directly in logic but represents 
    /// the parent node connection in the graph.
    /// </summary>
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;

    /// <summary>
    /// Output port for the child node. This value is not used directly in logic but represents 
    /// the child node connection in the graph.
    /// </summary>
    [Output(ShowBackingValue.Never), SerializeField] private int m_Children;

    /// <summary>
    /// Executes the child node and inverts its result.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this behavior tree.</param>
    /// <returns>
    /// A <see cref="CAD_NodeStateBT"/> indicating the inverted result of the child node's execution.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Execute the single connected child node.
        CAD_NodeStateBT state = GetConnectedChildren()[0].Execute(tankAI);

        // Invert the child node's result.
        switch (state)
        {
            case CAD_NodeStateBT.Running:
                return CAD_NodeStateBT.Running;
            case CAD_NodeStateBT.Success:
                return CAD_NodeStateBT.Failure;
            case CAD_NodeStateBT.Failure:
                return CAD_NodeStateBT.Success;
            // Default fallback case for unexpected states.
            default:
                return CAD_NodeStateBT.Failure;
        }
    }
}
