using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The decision processor that checks all rules every update and selects which action to perform.
/// </summary>
public class CAD_RulesEngine
{
    private CAD_SmartTankRBS m_TankAI;
    private List<CAD_Rule> m_Rules = new List<CAD_Rule>();

    public CAD_RulesEngine(CAD_SmartTankRBS tankAI)
    {
        m_TankAI = tankAI;
    }

    /// <summary>
    /// Add a rule to the engine.
    /// </summary>
    public void AddRule(CAD_Rule rule)
    {
        m_Rules.Add(rule);
        m_Rules = m_Rules.OrderBy(r => r.Priority).ToList();
    }

    /// <summary>
    /// Check conditions and perform the best-matching action.
    /// </summary>
    public void Update()
    {
        foreach (var rule in m_Rules)
        {
            if (rule.Condition(m_TankAI))
            {
                rule.Action(m_TankAI);
                break; // Found a rule to execute, stop checking others.
            }
        }
    }
}