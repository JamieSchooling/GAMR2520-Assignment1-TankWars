using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CAD_Action : ScriptableObject
{
    public abstract void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase);
}
