using UnityEngine;

[CreateAssetMenu(fileName = "Collect Health Action", menuName = "AI/RBS/Actions/Collect Health")]
public class CAD_CollectHealthAction : CAD_Action
{
    /// <summary>
    /// Moves the tank towards the nearest health consumable.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.NearestHealthConsumable.transform.position);
    }
}
