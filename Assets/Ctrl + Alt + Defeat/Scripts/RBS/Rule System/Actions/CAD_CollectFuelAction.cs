using UnityEngine;

[CreateAssetMenu(fileName = "Collect Fuel Action", menuName = "AI/RBS/Actions/Collect Fuel")]
public class CAD_CollectFuelAction : CAD_Action
{
    /// <summary>
    /// Moves the tank towards the nearest fuel consumable.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.NearestFuelConsumable.transform.position);
    }
}
