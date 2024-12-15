using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Holds a collection of conditions and logical operators to chain together, forming a final conditon.
/// </summary>
[Serializable]
public class CAD_ConditionGroup
{
    /// <summary>
    /// A list of individual conditions.
    /// </summary>
    [SerializeField] private List<CAD_Condition> m_Conditions = new();

    /// <summary>
    /// A list of logical operators.
    /// </summary>
    [SerializeField] private List<CAD_LogicalOperator> m_Operators = new();

    /// <summary>
    /// Returns all individual conditions.
    /// </summary>
    public List<CAD_Condition> Conditions => m_Conditions;

    /// <summary>
    /// Returns all logical operators.
    /// </summary>
    public List<CAD_LogicalOperator> Operators => m_Operators;

    /// <summary>
    /// Evaluates all individual conditions to build a final evaluation using all logical operators.
    /// </summary>
    /// <param name="knowledgeBase">The knowledge base to evaluate conditions from.</param>
    /// <returns>The final result of this condition group.</returns>
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

    /// <summary>
    /// Evaluates an individual condition from a knowledge base.
    /// </summary>
    /// <param name="conditionName">The name of the condition, used to retrieve the condition value from the knowledge base.</param>
    /// <param name="knowledgeBase">The knowledge base to evaluate the condition from.</param>
    /// <returns></returns>
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

/// <summary>
/// Logical operators that can be used when chaining conditions together.
/// </summary>
[Serializable]
public enum CAD_LogicalOperator { AND, OR }