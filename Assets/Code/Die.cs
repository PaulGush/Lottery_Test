using System;
using Code.Scenarios;
using UnityEngine;

namespace Code
{
    public class Die : MonoBehaviour
    {
        [SerializeField] private ScenarioManager m_scenarioManager;
        private Vector3 m_initialPosition;
        private Quaternion m_initialRotation;
        void Start()
        {
            m_initialPosition = transform.position;
            m_initialRotation = transform.rotation;
            
            ScenarioManager.OnRoll += ScenarioManager_OnRoll;
        }

        private void ScenarioManager_OnRoll() => RollDice();

        private void RollDice() => transform.SetPositionAndRotation(m_initialPosition, m_initialRotation);

        private void OnMouseDown() => m_scenarioManager?.Roll();
    }
}
