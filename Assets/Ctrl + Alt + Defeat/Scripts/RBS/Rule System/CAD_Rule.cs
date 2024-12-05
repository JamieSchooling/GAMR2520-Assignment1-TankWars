using System;

/// <summary>
/// Represents a single rule. Each rule has a condition function and an action function.
/// If the condition returns true, the action should be performed.
/// </summary>
public class CAD_Rule
{
    public Func<CAD_SmartTankRBS, bool> Condition { get; private set; }
    public Action<CAD_SmartTankRBS> Action { get; private set; }
    public int Priority { get; private set; }

    public CAD_Rule(Func<CAD_SmartTankRBS, bool> condition, Action<CAD_SmartTankRBS> action, int priority)
    {
        Condition = condition;
        Action = action;
        Priority = priority;
    }
}
