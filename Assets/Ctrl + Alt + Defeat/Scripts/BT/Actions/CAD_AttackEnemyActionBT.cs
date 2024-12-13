using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/BT/Actions")]
public class CAD_AttackEnemyActionBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        Vector3 position = tankAI.TanksFound.First().Key.transform.position;
        tankAI.Attack(position);

        return CAD_NodeStateBT.Success;
    }
}
