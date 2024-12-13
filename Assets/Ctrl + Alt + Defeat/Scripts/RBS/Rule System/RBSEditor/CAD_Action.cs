using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CAD_Action : ScriptableObject
{
    /// <summary>
    /// creates an abstract function for executing a command, using the tank and databse as its parameters
    /// will be re-defined in other scripts
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public abstract void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase);
}
