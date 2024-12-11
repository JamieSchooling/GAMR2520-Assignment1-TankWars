using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Represents a state where the AI-controlled tank retreats until a safe distance from the assailant.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Retreat State")]
public class CAD_RetreatState : CAD_State
{
    /// <summary>
    /// Stores the last known position of the enemy tank.
    /// </summary>
    private Vector3 m_EnemyPos;

    /// <summary>
    /// Holds a Positive x offset
    /// </summary>
    Vector3 m_PosXOffset = new Vector3(25, 0, 0);

    /// <summary>
    /// Holds a Negative x offset
    /// </summary>
    Vector3 m_NegXOffset = new Vector3(-25, 0, 0);

    /// <summary>
    /// Holds a Positive z offset
    /// </summary>
    Vector3 m_PosZOffset = new Vector3(0, 0, 25);

    /// <summary>
    /// Holds a Negative z offset
    /// </summary>
    Vector3 m_NegZOffset = new Vector3(0, 0, -25);

    /// <summary>
    /// Holds the current waypint game object
    /// </summary>
    private GameObject m_CurrentWaypoint;

    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {

    }

    /// <summary>
    ///  Called every frame to update the state behavior. Goes to the opposite side from enemy tank's position.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
    {
        //Creates the gameobject for the safest position
        GameObject safestPos = new GameObject("SafestPos");
        //Inverts the location of the enemy so the safeset postion is the opposite side of the map
        safestPos.transform.position = m_EnemyPos * -1;
        //Sets the safest position to the gameobject
        tankAI.LastKnownSafestPos = safestPos;
        //Checks if the tank can see the enemy bases
        if (tankAI.VisibleEnemyBases.Count > 0)
        {
            //Checks if the tank is close enough to the enemy bases
            if (Vector3.Distance(tankAI.transform.position, tankAI.VisibleEnemyBases.First().Key.transform.position) < 50.0f)
            {
                //The tank moves towards the centre of the map so as to not crash into the enemy bases
                Vector3 enemyBaseOffset = tankAI.VisibleEnemyBases.First().Key.transform.position + m_PosXOffset + m_PosZOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(enemyBaseOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
        }
        //Checks if the tank can see the friendly bases
        if (tankAI.FriendlyBases.Count > 0)
        {
            //Gets the closest friendly base out of the ones remaining
            var closestFriendlyBase = tankAI.FriendlyBases.OrderBy(b => b.Value).First().Key;
            //Checks if the tank is close enough to the closest base
            if (Vector3.Distance(tankAI.transform.position, closestFriendlyBase.transform.position) < 50.0f)
            {
                //The tank moves towards the centre of the map so as to not crash into the friebdky bases
                Vector3 friendlyBaseOffset = closestFriendlyBase.transform.position + m_NegXOffset + m_NegZOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(friendlyBaseOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
        }
        //Checks if the tank can see the enemy tank
        if (tankAI.EnemyTank)
        {
            // Checks if the tank is far enough from the enemy tank
            if (Vector3.Distance(tankAI.transform.position, tankAI.EnemyTank.transform.position) >= 50.0f)
            {
                //Calls the function to move to the safest point
                GoToSafePos(tankAI);
                return;
            }
            // Checks the tank's x poition against the enemy tank's x position
            if (tankAI.transform.position.x > tankAI.EnemyTank.transform.position.x)
            {
                //Moves the tank in a positive x to get away from the enemy tank
                Vector3 tankOffset = tankAI.EnemyTank.transform.position + m_PosXOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(tankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
            // Checks the tank's x poition against the enemy tank's x position
            if (tankAI.transform.position.x < tankAI.EnemyTank.transform.position.x)
            {
                //Moves the tank in a negative x to get away from the enemy tank
                Vector3 tankOffset = tankAI.EnemyTank.transform.position + m_NegXOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(tankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
            // Checks the tank's z poition against the enemy tank's z position
            if (tankAI.transform.position.z > tankAI.EnemyTank.transform.position.z)
            {
                //Moves the tank in a positive z to get away from the enemy tank
                Vector3 tankOffset = tankAI.EnemyTank.transform.position + m_PosZOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(tankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
            // Checks the tank's z poition against the enemy tank's z position
            if (tankAI.transform.position.z < tankAI.EnemyTank.transform.position.z)
            {
                //Moves the tank in a negative z to get away from the enemy tank
                Vector3 tankOffset = tankAI.EnemyTank.transform.position + m_NegZOffset;
                m_CurrentWaypoint = tankAI.CreateWaypoint(tankOffset);
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, 1f);
                Destroy(m_CurrentWaypoint);
            }
        }
        //Calls the GoToSafePos function
        GoToSafePos(tankAI);
    }

    private void GoToSafePos(CAD_SmartTankFSM tankAI)
    {
        //Moves the tank to the last known safepoint
        tankAI.FollowPathToWorldPoint(tankAI.LastKnownSafestPos, 1f);
        //If the tank sees the enemy, updates the enemy position
        if (tankAI.EnemyTank) m_EnemyPos = tankAI.EnemyTank.transform.position;
    }

    public override void OnStateExit(CAD_SmartTankFSM tankAI)
    {
        //Changes the last known enemy position to be accurate
        GameObject lastEnemyPos = new GameObject("LastEnemyPos");
        lastEnemyPos.transform.position = m_EnemyPos;
        tankAI.LastKnownEnemyPos = lastEnemyPos;
        Destroy(lastEnemyPos);
    }

    /// <summary>
    /// Creates list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// Should the tank be at the safe position, we move to the resource gathering state
    /// Should the tanks fuel be too low, we move to the resource gathering state
    /// Should the tank see fuel, we move to the resource gathering state
    /// This is done this way to hold priority on certain clauses even though they all go to the same state
    /// </summary>
    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Safe Distance", tankAI => Vector3.Distance(tankAI.transform.position, tankAI.LastKnownSafestPos.transform.position) < 25.0f),
            new CAD_Transition("Fuel too Low", tankAI => tankAI.Fuel <= 30.0f),
            new CAD_Transition("Fuel Spotted", tankAI => tankAI.VisibleConsumables.Where(c => c.Key.CompareTag("Fuel")).Count() > 0)
        };
    }
}
