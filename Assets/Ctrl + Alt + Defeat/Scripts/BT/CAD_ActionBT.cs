using UnityEngine;

public abstract class CAD_ActionBT : ScriptableObject
{
    public abstract CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI);
}
