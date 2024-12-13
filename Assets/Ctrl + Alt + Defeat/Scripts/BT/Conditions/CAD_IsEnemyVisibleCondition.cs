using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAD_IsEnemyVisibleCondition : CAD_ConditionBT
{
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        return tankAI.TanksFound.Count > 0;
    }
}
