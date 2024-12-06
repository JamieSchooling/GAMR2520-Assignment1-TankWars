using System;
using System.Collections.Generic;

/// <summary>
/// Represents a single rule. Each rule has a condition function and an action function.
/// If the condition returns true, the action should be performed.
/// </summary>
public class CAD_Rule
{
    public struct ConditionInfo
    {
        public string Name;
        public Func<CAD_SmartTankRBS, bool> Check;
    }

    public ConditionInfo Condition { get; private set; }
    public Action<CAD_SmartTankRBS> Action { get; private set; }
    public int Priority { get; private set; }

    public CAD_Rule(string conditionName, Func<CAD_SmartTankRBS, bool> condition, Action<CAD_SmartTankRBS> action, int priority)
    {
        Condition = new ConditionInfo
        {
            Name = conditionName,
            Check = condition,
        };
        Action = action;
        Priority = priority;
    }
}
