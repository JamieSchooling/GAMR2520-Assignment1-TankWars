using UnityEngine;

[CreateAssetMenu(fileName = "Approach Base Action", menuName = "AI/RBS/Actions/Aprroach Base")]
public class CAD_ApproachBaseAction : CAD_Action
{
    /// <summary>
    /// Moves the tank towards the enemy's base.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.EnemyBasePosition);
    }
}
