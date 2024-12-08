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
    private int m_WaypointIndex = 0;

    private float m_CurrentMoveSpeed = 1.0f;

    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {
        //Makes sure it starts at the closest resource waypoint
        float closestDistance = float.PositiveInfinity;
        for (int i = 0; i < m_ResourceWaypoints.Length; i++)
        {
            float currentDistance = Vector3.Distance(tankAI.transform.position, m_ResourceWaypoints[i]);
            if (currentDistance < closestDistance)
            {
                m_WaypointIndex = i;
                closestDistance = currentDistance;
            }
        }
        m_CurrentWaypoint = tankAI.CreateWaypoint(m_ResourceWaypoints[m_WaypointIndex]);
        tankAI.StartCoroutine(UpdateWaypoint(tankAI));
    }

    /// <summary>
    /// Determines what consumables to find based on current resource levels.
    /// Prioritises Health as long as Fuel can last, the Fuel, then Ammo.
    /// Ignores Health if it is max
    /// Ignores Ammo when above 5 shots
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
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

        m_CurrentMoveSpeed = (tankAI.Fuel >= 40) ? 1.0f : 0.5f;
        m_CurrentMoveSpeed = (tankAI.Fuel >= 10) ? m_CurrentMoveSpeed : 0.0f;

        FindConsumables(tankAI, consumablesToFind);
    }


    /// <summary>
    /// Finds the closest consumable out of the required types, and if none are found, follows a path to a random world point.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    /// <param name="consumableTypes">List of tags corresponding to the types of consumables needing found.</param>
    private void FindConsumables(CAD_SmartTankFSM tankAI, List<string> consumableTypes)
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
                tankAI.FollowPathToWorldPoint(consumable, m_CurrentMoveSpeed);
            }
            else
            {
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, m_CurrentMoveSpeed);
            }
        }
        else
        {
            tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, m_CurrentMoveSpeed);
        }
    }

    public override void OnStateExit(CAD_SmartTankFSM tankAI)
    {
        // TODO: Implement OnStateExit
    }

    /// <summary>
    /// Updates the waypoint index asynchronously
    /// </summary>
    /// <param name="tankAI"></param>
    /// <returns></returns>
    private IEnumerator UpdateWaypoint(CAD_SmartTankFSM tankAI)
    {
        while (true)
        {
            if (Vector3.Distance(tankAI.transform.position, m_CurrentWaypoint.transform.position) < 25.0f)
            {
                m_WaypointIndex++;
                if (m_WaypointIndex >= m_ResourceWaypoints.Count())
                {
                    m_WaypointIndex = 0;
                }

                Destroy(m_CurrentWaypoint);
                m_CurrentWaypoint = tankAI.CreateWaypoint(m_ResourceWaypoints[m_WaypointIndex]);
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
            new CAD_Transition("Enough to Attack Enemy", tankAI => ((tankAI.Health >= 60.0f && tankAI.Ammo > 0.0f 
            && tankAI.Fuel > 50.0f) || tankAI.Fuel <= 10.0f) && tankAI.EnemyTank),
            new CAD_Transition("Enough to Attack Base", tankAI => tankAI.Health > 20.0f && tankAI.Ammo > 0.0f
            && tankAI.Fuel > 50.0f && tankAI.VisibleEnemyBases.Count > 0.0f),
            new CAD_Transition("Retreat for Safety", tankAI => tankAI.EnemyTank && tankAI.Fuel >= 70.0f)
        };
    }
}
