using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAD_IsLowFuelCondition : CAD_ConditionBT
{
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        return tankAI.FuelLevel <= 30.0f;
    }
}
