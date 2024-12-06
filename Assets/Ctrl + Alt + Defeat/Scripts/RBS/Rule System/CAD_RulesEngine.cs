using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The decision processor that checks all rules every update and selects which action to perform.
/// </summary>
public class CAD_RulesEngine
{
    private CAD_SmartTankRBS m_TankAI;
    private CAD_Rules m_Rules;

    public CAD_RulesEngine(CAD_SmartTankRBS tankAI, CAD_Rules rules)
    {
        m_TankAI = tankAI;
        m_Rules = rules;
    }

    ///// <summary>
    ///// Add a rule to the engine.
    ///// </summary>
    //public void AddRule(CAD_Rule rule)
    //{
    //    m_Rules.Add(rule);
    //    m_Rules = m_Rules.OrderBy(r => r.Priority).ToList();
    //}

    /// <summary>
    /// Check conditions and perform the best-matching action.
    /// </summary>
    public void Update(CAD_KnowledgeBase knowledgeBase)
    {
        foreach (var rule in m_Rules.Rules)
        {
            Debug.Log($"{rule.Name}: {rule.conditions.Evaluate(knowledgeBase)}");
            if (rule.conditions.Evaluate(knowledgeBase))
            {
                rule.action.Execute(m_TankAI, knowledgeBase);
                break; // Found a rule to execute, stop checking others.
            }
        }
    }
}