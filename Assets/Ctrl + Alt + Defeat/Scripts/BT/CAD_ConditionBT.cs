using UnityEngine;

public abstract class CAD_ConditionBT : ScriptableObject
{
    public abstract bool Evaluate(CAD_SmartTankBT tankAI);
}
