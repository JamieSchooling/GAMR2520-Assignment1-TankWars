using UnityEngine;

[CreateAssetMenu(fileName = "Collect Health Action", menuName = "AI/RBS/Actions/Collect Health")]
public class CAD_CollectHealthAction : CAD_Action
{
    /// <summary>
    /// tank goes towards that last known position of the health consumable
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.NearestHealthConsumable.transform.position);
    }
}
