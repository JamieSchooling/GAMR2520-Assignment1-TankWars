using UnityEngine;

[System.Serializable]
public class CAD_Condition
{
    [SerializeField] private string m_Name;
    [SerializeField] private bool m_Negate;

    public string Name => m_Name;
    public bool Negate => m_Negate;
}
