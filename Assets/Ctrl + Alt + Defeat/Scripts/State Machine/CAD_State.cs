using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all states used in a StateMachine. Inherits from ScriptableObject for serialization purposes.
/// </summary>
public abstract class CAD_State : ScriptableObject
{
    /// <summary>
    /// List of transition conditions to exit state.
    /// </summary>
    public List<CAD_Transition> Transitions { get; protected set; }

    /// <summary>
    /// Called when this state is transitioned into.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public abstract void OnStateEnter(CAD_SmartTank tankAI);
        
    /// <summary>
    /// Called when this state is updated.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public abstract void OnStateUpdate(CAD_SmartTank tankAI);
        
    /// <summary>
    /// Called after this state meets a transition condition.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public abstract void OnStateExit(CAD_SmartTank tankAI);
}
