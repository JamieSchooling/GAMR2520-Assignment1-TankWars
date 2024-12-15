using UnityEngine;

[CreateAssetMenu(fileName = "Collect Ammo Action", menuName = "AI/RBS/Actions/Collect Ammo")]
public class CAD_CollectAmmoAction : CAD_Action
{
    /// <summary>
    /// Moves the tank towards the nearest ammo consumable.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.NearestAmmoConsumable.transform.position);
    }
}
