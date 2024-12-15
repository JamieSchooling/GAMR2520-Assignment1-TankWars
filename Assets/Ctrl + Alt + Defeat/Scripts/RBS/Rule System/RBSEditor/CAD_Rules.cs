using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a ruleset that an AI can follow.
/// </summary>
[CreateAssetMenu(menuName = "AI/Rules")]
public class CAD_Rules : ScriptableObject
{
    /// <summary>
    /// A list of all rules to be evaluated.
    /// </summary>
    [SerializeField] private List<CAD_Rule> m_Rules = new List<CAD_Rule>();

    /// <summary>
    /// Returns the list of rules.
    /// </summary>
    public List<CAD_Rule> Rules => m_Rules;
}
