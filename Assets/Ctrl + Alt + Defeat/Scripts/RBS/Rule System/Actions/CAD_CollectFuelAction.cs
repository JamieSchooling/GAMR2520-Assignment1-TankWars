using UnityEngine;

[CreateAssetMenu(fileName = "Collect Fuel Action", menuName = "AI/RBS/Actions/Collect Fuel")]
public class CAD_CollectFuelAction : CAD_Action
{
    /// <summary>
    /// tank goes towards that last known position of the fuel consumable
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.NearestFuelConsumable.transform.position);
    }
}
