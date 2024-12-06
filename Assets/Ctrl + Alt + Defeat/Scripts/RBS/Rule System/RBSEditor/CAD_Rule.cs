using System;

[Serializable]
public class CAD_Rule
{
    public string Name;
    public CAD_ConditionGroup conditions; // or a list of conditions if needed
    public CAD_Action action;
}
