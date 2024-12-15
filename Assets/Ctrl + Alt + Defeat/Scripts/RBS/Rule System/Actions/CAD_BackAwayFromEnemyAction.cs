using UnityEngine;

[CreateAssetMenu(fileName = "Back Away From Enemy Action", menuName = "AI/RBS/Actions/Back Away From Enemy")]
public class CAD_BackAwayFromEnemyAction : CAD_Action
{
    [SerializeField] private float m_BackAwayDistance = 20.0f;

    /// <summary>
    /// Moves the tank away from the enemy tank.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        knowledgeBase.CurrentSearchWaypoint = knowledgeBase.EnemyPosition;
        knowledgeBase.TimeLastSeenEnemy = Time.time;
        tankAI.GoTo((knowledgeBase.NearestEnemyTank.transform.forward * m_BackAwayDistance) 
            + (knowledgeBase.NearestEnemyTank.transform.right * m_BackAwayDistance));
    }
}
