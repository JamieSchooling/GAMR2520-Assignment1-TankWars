using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/BT/Conditions")]
public class CAD_IsEnemyInRangeCondition : CAD_ConditionBT
{
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        if (tankAI.TanksFound.Count <= 0) return false;

        float distance = Vector3.Distance(tankAI.TanksFound.First().Key.transform.position,
                                            tankAI.transform.position);

        return distance < 25.0f;
    }
}
