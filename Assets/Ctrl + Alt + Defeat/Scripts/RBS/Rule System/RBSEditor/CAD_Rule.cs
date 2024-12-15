using System;
using UnityEngine;

/// <summary>
/// holds the name of the rule followed by the set of conditions it has to meet (m_Conditions) and the action that will get executed (m_Action)
/// </summary>
[Serializable]
public class CAD_Rule
{
    /// <summary>
    /// The name of this rule.
    /// </summary>
    [SerializeField] private string m_Name;

    /// <summary>
    /// The conditions needed to execute this rule's action.
    /// </summary>
    [SerializeField] private CAD_ConditionGroup m_Conditions;

    /// <summary>
    /// The action to execute when this rule's conditons have been met.
    /// </summary>
    [SerializeField] private CAD_Action m_Action;

    /// <summary>
    /// Returns this rule's name.
    /// </summary>
    public string Name => m_Name;

    /// <summary>
    /// Returns this rule's conditions.
    /// </summary>
    public CAD_ConditionGroup Conditions => m_Conditions;

    /// <summary>
    /// Returns this rule's action.
    /// </summary>
    public CAD_Action Action => m_Action;
}
