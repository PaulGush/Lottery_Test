using System.Collections.Generic;
using Code.Scenarios;
using UnityEngine;

namespace Code
{
    public class Die : MonoBehaviour
    {
        [SerializeField] private float m_rollSpeed = 10f;
        private bool m_isRolling = false;

        public Dictionary<Vector3, int> m_rotations;
        void Start()
        {
            ScenarioManager.OnRoll += ScenarioManager_OnRoll;
            ScenarioManager.OnLand += ScenarioManager_OnLand;
        }

        private void ScenarioManager_OnLand()
        {
            m_isRolling = false;
        }

        private void ScenarioManager_OnRoll() => m_isRolling = true;

        void Update()
        {
            if (m_isRolling)
            {
                transform.Rotate(Time.deltaTime * m_rollSpeed, Time.deltaTime * m_rollSpeed, Time.deltaTime * m_rollSpeed);
            }
        }

        private void LandOnSide(int side)
        {
            
        }
    }
}
