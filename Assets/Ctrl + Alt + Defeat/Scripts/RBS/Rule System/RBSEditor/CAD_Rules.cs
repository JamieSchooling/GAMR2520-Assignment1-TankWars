using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/DecisionRules")]
public class CAD_Rules : ScriptableObject
{
    [SerializeField] private List<CAD_Rule> m_Rules = new List<CAD_Rule>();
    [SerializeField] private CAD_KnowledgeBase m_KnowledgeBase = null;

    public List<CAD_Rule> Rules => m_Rules;
    public CAD_KnowledgeBase KnowledgeBase => m_KnowledgeBase;
}
