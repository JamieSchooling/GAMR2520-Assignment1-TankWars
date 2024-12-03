using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a state where the AI-controlled tank searches for consumables until at high enough stats. 
/// If none are found, the tank moves to a random world point.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Resource Gathering State")]

public class CAD_Resource_Gathering_State : CAD_State
{
    /// <summary>
    /// Holds the time in seconds since the last random path finding target was generated.
    /// </summary>
    private float m_CurrentTime = 0;

    public override void OnStateEnter(CAD_SmartTank tankAI)
    {

    }

    /// <summary>
    /// Determines what consumables to find based on current resource levels.
    /// Prioritises Health as long as Fuel can last, the Fuel, then Ammo.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTank tankAI)
    {
        List<string> consumablesToFind = new();

        if (tankAI.Health <= 30.0f && tankAI.Fuel >= 30.0f)
        {
            consumablesToFind.Add("Health");
        }
        else
        {
            consumablesToFind.Add("Fuel");
        }
        if (consumablesToFind[0] != "Health")
        {
            consumablesToFind.Add("Health");
        }
        if (tankAI.Fuel <= 50.0f && consumablesToFind[0] != "Fuel")
        {
            consumablesToFind.Add("Fuel");
        }
        if (tankAI.Ammo <= 4.0f)
        {
            consumablesToFind.Add("Ammo");
        }
        

        FindConsumables(tankAI, consumablesToFind);
    }


    /// <summary>
    /// Finds the closest consumable out of the required types, and if none are found, follows a path to a random world point.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    /// <param name="consumableTypes">List of tags corresponding to the types of consumables needing found.</param>
    private void FindConsumables(CAD_SmartTank tankAI, List<string> consumableTypes)
    {
        if (tankAI.VisibleConsumables.Count > 0)
        {
            // Filter consumables matching the required types
            var potentialConsumables = tankAI.VisibleConsumables
                .Where(c => consumableTypes.Any(type => c.Key.CompareTag(type)))
                .OrderBy(c => c.Value) // Order by distance
                .ToList();

            if (potentialConsumables.Count > 0)
            {
                // Get the closest consumable
                GameObject consumable = potentialConsumables.First().Key;
                tankAI.FollowPathToWorldPoint(consumable, 1f);
            }
            else
            {
                tankAI.FollowPathToRandomWorldPoint(1f);
            }
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
    /// Creates list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Enough Resources", tankAI => tankAI.Health >= 60.0f && tankAI.Ammo >= 5.0f && tankAI.Fuel >= 70.0f),
            new CAD_Transition("Retreat for Safety", tankAI => tankAI.EnemyTank && tankAI.Fuel >= 70.0f)
        };
    }

}


