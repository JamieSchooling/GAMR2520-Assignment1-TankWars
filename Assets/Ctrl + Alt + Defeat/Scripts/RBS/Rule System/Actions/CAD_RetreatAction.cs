using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RetreatAction", menuName = "AI/RBS/Actions/Retreat")]
public class CAD_RetreatAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        knowledgeBase.CurrentSearchWaypoint = knowledgeBase.EnemyPosition*-1;
        knowledgeBase.TimeLastSeenEnemy = Time.time;
        tankAI.GoTo(knowledgeBase.CurrentSearchWaypoint);
    }
}
