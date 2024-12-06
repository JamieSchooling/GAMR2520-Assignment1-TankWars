using System;
using UnityEngine;

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
