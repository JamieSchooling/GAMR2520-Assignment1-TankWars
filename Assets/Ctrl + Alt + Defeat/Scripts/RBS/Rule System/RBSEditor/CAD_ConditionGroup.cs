using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Creates an object that holds conditions and operators to make a bigger condition
/// </summary>
[Serializable]
public class CAD_ConditionGroup
{
    [SerializeField] private List<CAD_Condition> m_Conditions = new();
    [SerializeField] private List<CAD_LogicalOperator> m_Operators = new();

    public List<CAD_Condition> Conditions => m_Conditions;
    public List<CAD_LogicalOperator> Operators => m_Operators;

    public bool Evaluate(CAD_KnowledgeBase knowledgeBase)
    {
        bool result = EvaluateCondition(Conditions[0].Name, knowledgeBase);
        result = Conditions[0].Negate ? !result : result;

        for (int i = 1; i < Conditions.Count; i++)
        {
            if (Operators.Count < 1) break;

            switch (Operators[i - 1])
            {
                case CAD_LogicalOperator.AND:
                    if (!result) return false;
                    bool evaluation = EvaluateCondition(Conditions[i].Name, knowledgeBase);
                    result = result && (Conditions[i].Negate ? !evaluation : evaluation); break;
                case CAD_LogicalOperator.OR:
                    if (result) return true;
                    evaluation = EvaluateCondition(Conditions[i].Name, knowledgeBase);
                    result = result || (Conditions[i].Negate ? !evaluation : evaluation); break;
            }
        }

        return result;
    }

    private bool EvaluateCondition(string conditionName, CAD_KnowledgeBase knowledgeBase)
    {
        // Get the type of the KnowledgeBase
        Type kbType = knowledgeBase.GetType();

        // Get the PropertyInfo for the given condition name
        PropertyInfo propInfo = kbType.GetProperty(conditionName, BindingFlags.Public | BindingFlags.Instance);

        if (propInfo == null)
        {
            Debug.LogWarning($"Condition '{conditionName}' not found in KnowledgeBase.");
            return false;
        }

        // Ensure the property is of type bool
        if (propInfo.PropertyType != typeof(bool))
        {
            Debug.LogWarning($"Condition '{conditionName}' is not of type bool.");
            return false;
        }

        // Get the value of the property
        bool conditionValue = (bool)propInfo.GetValue(knowledgeBase, null);
        return conditionValue;
    }
}

[Serializable]
public enum CAD_LogicalOperator { AND, OR }