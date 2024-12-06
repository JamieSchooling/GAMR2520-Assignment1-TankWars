using UnityEngine;

[CreateAssetMenu(fileName = "Approach Base Action", menuName = "AI/RBS/Actions/Aprroach Base")]
public class CAD_ApproachBaseAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.EnemyBasePosition);
    }
}
