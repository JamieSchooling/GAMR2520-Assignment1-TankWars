using UnityEditor;
using UnityEngine;

/// <summary>
/// Represents a condition node in a behavior tree.
/// The condition node evaluates a specified condition and determines whether to proceed to its child node based on the evaluation result.
/// </summary>
[CreateNodeMenu("Behaviour Tree/Condition")]
public class CAD_ConditionNode : CAD_NodeBT
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
    /// The script representing the condition logic to evaluate.
    /// </summary>
    [SerializeField] private MonoScript m_Condition;

    /// <summary>
    /// The instantiated condition logic for evaluation.
    /// </summary>
    private CAD_ConditionBT m_ConditionInstance;

    /// <summary>
    /// Initializes the condition node by creating an instance of the specified condition class.
    /// </summary>
    protected override void Init()
    {
        m_ConditionInstance = CreateInstance(m_Condition.GetClass()) as CAD_ConditionBT;
    }

    /// <summary>
    /// Executes the condition node by evaluating the condition and proceeding based on the result.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this behavior tree.</param>
    /// <returns>
    /// A <see cref="CAD_NodeStateBT"/> indicating the result of the condition evaluation:
    /// - If the condition evaluates to true, executes the first connected child node (if any) or returns <c>Success</c>.
    /// - If the condition evaluates to false, returns <c>Failure</c>.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Evaluate the condition.
        if (m_ConditionInstance.Evaluate(tankAI))
        {
            // If the condition is true, execute the connected child node or return Success if no child exists.
            return GetConnectedChildren().Count > 0 ? GetConnectedChildren()[0].Execute(tankAI) : CAD_NodeStateBT.Success;
        }
        else
        {
            // If the condition is false, return Failure.
            return CAD_NodeStateBT.Failure;
        }
    }
}
