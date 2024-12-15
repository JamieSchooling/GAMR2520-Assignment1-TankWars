using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RetreatAction", menuName = "AI/RBS/Actions/Retreat")]
public class CAD_RetreatAction : CAD_Action
{
    /// <summary>
    /// Moves the tank to a safe position, away from the enemy tank.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        knowledgeBase.CurrentSearchWaypoint = knowledgeBase.EnemyPosition*-1;
        knowledgeBase.TimeLastSeenEnemy = Time.time;
        tankAI.GoTo(knowledgeBase.CurrentSearchWaypoint);
    }
}
