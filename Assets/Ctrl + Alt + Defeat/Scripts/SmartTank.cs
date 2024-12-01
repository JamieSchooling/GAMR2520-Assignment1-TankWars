using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CAD
{
    public class SmartTank : AITank
    {
        [SerializeField] private StateMachineGraph m_StateMachineGraph;

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

        private StateMachine m_StateMachine;

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
            InitialiseStateMachine();
        }

        public override void AITankUpdate()
        {
            m_StateMachine.Update();
        }

        public override void AIOnCollisionEnter(Collision collision)
        {
            // TODO: Implement Collision Response
        }

        private void InitialiseStateMachine()
        {
            m_StateMachine = new(this);

            m_StateMachine.Start(m_StateMachineGraph);
        }
    }
}
