using UnityEngine;

[CreateAssetMenu(fileName = "Approach Base Action", menuName = "AI/RBS/Actions/Aprroach Base")]
public class CAD_ApproachBaseAction : CAD_Action
{
    /// <summary>
    /// moves the tank to the last known enemy base position
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.EnemyBasePosition);
    }
}
