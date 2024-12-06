using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/DecisionRules")]
public class CAD_Rules : ScriptableObject
{
    [SerializeField] private List<CAD_Rule> rules = new List<CAD_Rule>();
    [SerializeField] private CAD_KnowledgeBase knowledgeBase = null;

    public List<CAD_Rule> Rules => rules;
    public CAD_KnowledgeBase KnowledgeBase => knowledgeBase;
}
