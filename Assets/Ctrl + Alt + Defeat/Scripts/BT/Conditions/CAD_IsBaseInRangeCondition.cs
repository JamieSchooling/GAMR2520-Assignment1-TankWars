using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/BT/Conditions")]
public class CAD_IsBaseInRangeCondition : CAD_ConditionBT
{
    public override bool Evaluate(CAD_SmartTankBT tankAI)
    {
        float distance = Vector3.Distance(tankAI.BasesFound.OrderBy(b => b.Value).First().Key.transform.position,
                                            tankAI.transform.position);

        return distance < 20.0f;
    }
}
