public abstract class CAD_BTNode
{
    public abstract CAD_BTState Execute(CAD_SmartTankBT tankAI);
}

public enum CAD_BTState
{
    Success,
    Failure,
    Running
}