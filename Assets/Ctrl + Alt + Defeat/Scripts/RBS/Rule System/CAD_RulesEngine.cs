using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The decision processor that checks all rules every update and selects which action to perform.
/// </summary>
public class CAD_RulesEngine
{
    /// <summary>
    /// Instance of the Smart Tank AI.
    /// </summary>
    private CAD_SmartTankRBS m_TankAI;

    /// <summary>
    /// The rules to process.
    /// </summary>
    private CAD_Rules m_Rules;

    public CAD_RulesEngine(CAD_SmartTankRBS tankAI, CAD_Rules rules)
    {
        m_TankAI = tankAI;
        m_Rules = rules;
    }

    /// <summary>
    /// Check conditions and perform the best-matching action.
    /// </summary>
    public void Update(CAD_KnowledgeBase knowledgeBase)
    {
        foreach (var rule in m_Rules.Rules)
        {
            Debug.Log($"{rule.Name}: {rule.Conditions.Evaluate(knowledgeBase)}");
            if (rule.Conditions.Evaluate(knowledgeBase))
            {
                rule.Action.Execute(m_TankAI, knowledgeBase);
                break; // Found a rule to execute, stop checking others.
            }
        }
    }
}