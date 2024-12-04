using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a state where the AI-controlled tank searches for consumables until at high enough stats. 
/// If none are found, the tank moves to the following world points:
///(-74.1999969,14.3393097,64.6999969) = Top Left Tree
///(68.5, 14.3393097, 74.5) = Home Base
///(63.2999992,14.3393097,-32.5999985) = Above Bottom Right Tree
///(32.0999985,14.3393097,-67.9000015) = Left of Bottom Right Tree
///(-59.0999985,14.3393097,-87.1999969) = Enemy Base
///(-67.5,14.3393097,-39.7000008) = Middle Left
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Resource Gathering State")]

public class CAD_Resource_Gathering_State : CAD_State
{
    /// <summary>
    /// Holds all the positions for the resource spawn points.
    /// </summary>
    [SerializeField] private Vector3[] m_ResourceWaypoints;
    /// <summary>
    /// Holds the current waypint game object
    /// </summary>
    private GameObject m_CurrentWaypoint;
    /// <summary>
    /// Holds the waypoint index (where in the list of waypoints)
    /// </summary>
    private int WaypointIndex = 0;

    public override void OnStateEnter(CAD_SmartTank tankAI)
    {
        //Makes sure it starts at the closest resource waypoint
        float beststart = float.PositiveInfinity;
        for (int i = 0; i < m_ResourceWaypoints.Length; i++)
        {
            float currentstart = Vector3.Distance(tankAI.transform.position, m_ResourceWaypoints[i]);
            if (currentstart < beststart)
            {
                WaypointIndex = i;
                beststart = currentstart;
            }
        }
        m_CurrentWaypoint = tankAI.CreateWaypoint(m_ResourceWaypoints[WaypointIndex]);
        tankAI.StartCoroutine(UpdateWaypoint(tankAI));
    }

    /// <summary>
    /// Determines what consumables to find based on current resource levels.
    /// Prioritises Health as long as Fuel can last, the Fuel, then Ammo.
    /// Ignores Health if it is max
    /// Ignores Ammo when above 5 shots
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTank tankAI)
    {
        List<string> consumablesToFind = new();

        if (tankAI.Health != 125.0f)
        {
            if (tankAI.Fuel >= 40.0f)
            {
                consumablesToFind.Add("Health");
            }
            else
            {
                consumablesToFind.Add("Fuel");
            }
            if (!consumablesToFind.Contains("Health"))
            {
                consumablesToFind.Add("Health");
            }
            if (!consumablesToFind.Contains("Fuel"))
            {
                consumablesToFind.Add("Fuel");
            }
        }
        else
        {
            consumablesToFind.Add("Fuel");
        }
        if (tankAI.Ammo <= 5.0f)
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
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
            }
        }
        else
        {
            tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
        }
    }

    public override void OnStateExit(CAD_SmartTank tankAI)
    {
        // TODO: Implement OnStateExit
    }
    /// <summary>
    /// Updates the waypoint index asynchronously
    /// </summary>
    /// <param name="tankAI"></param>
    /// <returns></returns>
    private IEnumerator UpdateWaypoint(CAD_SmartTank tankAI)
    {
        while (true)
        {
            if (Vector3.Distance(tankAI.transform.position, m_CurrentWaypoint.transform.position) < 25.0f)
            {

                WaypointIndex++;
                if (WaypointIndex >= m_ResourceWaypoints.Count())
                {
                    WaypointIndex = 0;
                }

                Destroy(m_CurrentWaypoint);
                m_CurrentWaypoint = tankAI.CreateWaypoint(m_ResourceWaypoints[WaypointIndex]);
            }
            yield return new WaitForEndOfFrame();
        }
    }


    /// <summary>
    /// Creates list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Enough Resources", tankAI => tankAI.Health >= 60.0f && tankAI.Ammo >= 5.0f && tankAI.Fuel >= 70.0f),
            new CAD_Transition("Enough to Attack Enemy", tankAI => tankAI.Health >= 60.0f && tankAI.Ammo > 0.0f 
            && tankAI.Fuel > 50.0f && tankAI.EnemyTank),
            new CAD_Transition("Enough to Attack Base", tankAI => tankAI.Health > 20.0f && tankAI.Ammo > 0.0f
            && tankAI.Fuel > 50.0f && tankAI.VisibleEnemyBases.Count > 0.0f),
            new CAD_Transition("Retreat for Safety", tankAI => tankAI.EnemyTank && tankAI.Fuel >= 70.0f)
        };
    }

}


