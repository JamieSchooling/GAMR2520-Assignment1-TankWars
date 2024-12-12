using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAD_IsBaseVisibleCondition : CAD_ConditionBT
{
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        return tankAI.BasesFound.Count > 0;
    }
}
