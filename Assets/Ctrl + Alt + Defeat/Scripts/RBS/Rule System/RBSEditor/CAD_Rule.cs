using System;
using UnityEngine;

/// <summary>
/// holds the name of the rule followed by the set of conditions it has to meet (m_Conditions) and the action that will get executed (m_Action)
/// </summary>
[Serializable]
public class CAD_Rule
{
    [SerializeField] private string m_Name;
    [SerializeField] private CAD_ConditionGroup m_Conditions;
    [SerializeField] private CAD_Action m_Action;

    public string Name => m_Name;
    public CAD_ConditionGroup Conditions => m_Conditions; // or a list of conditions if needed
    public CAD_Action Action => m_Action;
}
