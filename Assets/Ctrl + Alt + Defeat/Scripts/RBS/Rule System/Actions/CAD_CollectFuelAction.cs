using UnityEngine;

[CreateAssetMenu(fileName = "Collect Fuel Action", menuName = "AI/RBS/Actions/Collect Fuel")]
public class CAD_CollectFuelAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.NearestFuelConsumable.transform.position);
    }
}
