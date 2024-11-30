using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CAD
{
    public class SmartTank : AITank
    {
        private StateMachine m_StateMachine;

        private SearchState m_SearchState = new();
        private ChaseState m_ChaseState = new();
        private AttackState m_AttackState = new();
        private RetreatState m_RetreatState = new();

        public Dictionary<GameObject, float> VisibleConsumables => a_ConsumablesFound;
        public GameObject EnemyTank
        {
            get 
            {
                if (a_TanksFound.Count > 0) return a_TanksFound.First().Key;
                return null;
            }
        }

        public GameObject LastKnownEnemyPos
        {
            get => m_LastKnownEnemyPos;
            set
            {
                Destroy(m_LastKnownEnemyPos);
                m_LastKnownEnemyPos = value;
            }
        }

        public float Health => a_GetHealthLevel;
        public float Ammo => a_GetAmmoLevel;
        public float Fuel => a_GetFuelLevel;

        private GameObject m_LastKnownEnemyPos = null;

        public void GenerateNewRandomWorldPoint()
        {
            a_GenerateRandomPoint();
        }

        public void FollowPathToWorldPoint(GameObject pointInWorld, float normalizedSpeed)
        {
            a_FollowPathToPoint(pointInWorld, normalizedSpeed);
        }

        public void FollowPathToRandomWorldPoint(float normalizedSpeed)
        {
            a_FollowPathToRandomPoint(normalizedSpeed);
        }

        public void TurretFireAtPoint(GameObject pointInWorld)
        {
            a_FireAtPoint(pointInWorld);
        }

        public override void AITankStart()
        {
            m_SearchState.GetTransitions().Add(new Transition(m_RetreatState, tankAI => tankAI.Health <= 30.0f || tankAI.Ammo <= 4.0f || tankAI.Fuel <= 50.0f));
            m_SearchState.GetTransitions().Add(new Transition(m_ChaseState, tankAI => tankAI.EnemyTank));

            m_RetreatState.GetTransitions().Add(new Transition(m_ChaseState, tankAI => tankAI.EnemyTank));
            m_RetreatState.GetTransitions().Add(new Transition(m_SearchState, tankAI => !tankAI.EnemyTank && tankAI.Health > 30.0f && tankAI.Ammo > 4.0f && tankAI.Fuel > 50.0f));

            m_ChaseState.GetTransitions().Add(new Transition(m_RetreatState, tankAI => tankAI.Health <= 30.0f || tankAI.Ammo <= 4.0f || tankAI.Fuel <= 50.0f));
            m_ChaseState.GetTransitions().Add(new Transition(m_SearchState, tankAI => !tankAI.EnemyTank && tankAI.Health > 30.0f && tankAI.Ammo > 4.0f && tankAI.Fuel > 50.0f));
            m_ChaseState.GetTransitions().Add(new Transition(m_AttackState, tankAI => Vector3.Distance(transform.position, a_TanksFound.First().Key.transform.position) < 25.0f));

            m_AttackState.GetTransitions().Add(new Transition(m_RetreatState, tankAI => tankAI.Health <= 30.0f || tankAI.Ammo <= 4.0f || tankAI.Fuel <= 50.0f));
            m_AttackState.GetTransitions().Add(new Transition(m_SearchState, tankAI => !tankAI.EnemyTank && tankAI.Health > 30.0f && tankAI.Ammo > 4.0f && tankAI.Fuel > 50.0f));

            m_StateMachine = new(this);
            m_StateMachine.AddState(m_SearchState);
            m_StateMachine.AddState(m_ChaseState);
            m_StateMachine.AddState(m_AttackState);
            m_StateMachine.AddState(m_RetreatState);

            m_StateMachine.Start();
        }

        public override void AITankUpdate()
        {
            m_StateMachine.Update();
        }

        public override void AIOnCollisionEnter(Collision collision)
        {
            // TODO: Implement Collision Response
        }
    }
}
