using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a state where the AI-controlled tank searches for the last known enemy position or consumables. 
/// If neither is found, the tank moves to a random world point.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Search State")]
public class CAD_SearchState : CAD_State
{
    /// <summary>
    /// Holds the time in seconds since the last random path finding target was generated.
    /// </summary>
    private float m_CurrentTime;
    [HideInInspector]
    public List<Vector3> barrelPositiions;
    [HideInInspector]
    public List<Vector3> ammoPositiions;
    [HideInInspector]
    public List<Vector3> healthPositiions;

    /// <summary>
    /// Called when the state is entered. Initializes time tracking for the state.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance entering the state.</param>
    public override void OnStateEnter(CAD_SmartTank tankAI)
    {
        m_CurrentTime = 0;
    }

    /// <summary>
    /// The tank will search for the last known enemy position,
    /// approach consumables, or move to a random world point if neither are found.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTank tankAI)
    {
        if (tankAI.LastKnownEnemyPos)
        {
            if (Vector3.Distance(tankAI.transform.position, tankAI.LastKnownEnemyPos.transform.position) < 5.0f)
            {
                Destroy(tankAI.LastKnownEnemyPos);
                tankAI.LastKnownEnemyPos = null;
            }
            tankAI.FollowPathToWorldPoint(tankAI.LastKnownEnemyPos, 1f);
        }
        else if (tankAI.VisibleConsumables.Count > 0)
        {
            GameObject consumable = tankAI.VisibleConsumables.First().Key;
            switch (consumable.name)
            {
                case "Fuel":
                    if (tankAI.Fuel <= 75.0f)
                    {
                        tankAI.FollowPathToWorldPoint(consumable, 1f);
                    }
                    else
                    {
                        barrelPositiions.Add(consumable.GetComponent<Transform>().position);
                    }
                    break;
                case "Ammo":
                    if (tankAI.Ammo <= 8.0f)
                    {
                        tankAI.FollowPathToWorldPoint(consumable, 1f);
                    }
                    else
                    {
                        ammoPositiions.Add(consumable.GetComponent<Transform>().position);
                    }
                    break;
                case "Health":
                    if (tankAI.Health <= 80.0f)
                    {
                        tankAI.FollowPathToWorldPoint(consumable, 1f);
                    }
                    else
                    {
                        healthPositiions.Add(consumable.GetComponent<Transform>().position);
                    }
                    break;
            }
            m_CurrentTime += Time.deltaTime;
        }
        else
        {
            tankAI.FollowPathToRandomWorldPoint(1f);
        }

        m_CurrentTime += Time.deltaTime;
        if (m_CurrentTime > 10)
        {
            tankAI.GenerateNewRandomWorldPoint();
            m_CurrentTime = 0;
        }
    }

    public override void OnStateExit(CAD_SmartTank tankAI)
    {
        // TODO: Implement OnStateExit
    }

    /// <summary>
    /// Creates a list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Low Resources", tankAI => tankAI.Health <= 30.0f || tankAI.Ammo <= 4.0f || tankAI.Fuel <= 50.0f),
            new CAD_Transition("Tank Found", tankAI => tankAI.EnemyTank)
        };
    }
}
