using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class CAD_ConditionGroup
{
    public List<CAD_Condition> conditions = new();
    public List<CAD_LogicalOperator> operators;

    public bool Evaluate(CAD_KnowledgeBase knowledgeBase)
    {
        bool result = EvaluateCondition(conditions[0].Name, knowledgeBase);

        for (int i = 1; i < conditions.Count; i++)
        {
            switch (operators[i - 1])
            {
                case CAD_LogicalOperator.AND:
                    if (!result) return false;
                    bool evaluation = EvaluateCondition(conditions[i].Name, knowledgeBase);
                    result = result && (conditions[i].negate ? !evaluation : evaluation); break;
                case CAD_LogicalOperator.OR:
                    result = (conditions[i].negate ? !result : result) || EvaluateCondition(conditions[i].Name, knowledgeBase); break;
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