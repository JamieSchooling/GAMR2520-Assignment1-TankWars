using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CAD_IsHealthVisibleCondition : CAD_ConditionBT
{
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        return tankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Health")).Count() > 0;
    }
}
