using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all actions to be executed by rules.
/// </summary>
public abstract class CAD_Action : ScriptableObject
{
    /// <summary>
    /// Base method for executing an action.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public abstract void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase);
}
