using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Represents an action node in a behavior tree.
/// The action node performs a specific behavior defined by a provided action script.
/// </summary>
[CreateNodeMenu("Behaviour Tree/Action")]
public class CAD_ActionNode : CAD_NodeBT
{
    /// <summary>
    /// Input port for the parent node. This value is not used directly in logic but represents 
    /// the parent node connection in the graph.
    /// </summary>
    [Input(ShowBackingValue.Never), SerializeField] private int m_Parent;

    /// <summary>
    /// The script representing the action logic to execute.
    /// </summary>
    [SerializeField] private MonoScript m_Action;

    /// <summary>
    /// The instantiated action logic for execution.
    /// </summary>
    private CAD_ActionBT m_ActionInstance;

    /// <summary>
    /// Initializes the action node by creating an instance of the specified action class.
    /// </summary>
    protected override void Init()
    {
        if (m_Action == null) return;

        // Create an instance of the action class.
        m_ActionInstance = CreateInstance(m_Action.GetClass()) as CAD_ActionBT;
    }

    /// <summary>
    /// Executes the action node by invoking the logic of the associated action script.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this behavior tree.</param>
    /// <returns>
    /// A <see cref="CAD_NodeStateBT"/> indicating the result of the action's execution:
    /// - <c>Success</c> if the action completes successfully.
    /// - <c>Failure</c> if the action fails.
    /// - <c>Running</c> if the action is still in progress.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Execute the action logic.
        return m_ActionInstance.Execute(tankAI);
    }
}
