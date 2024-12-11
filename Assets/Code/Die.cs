using Code.Scenarios;
using UnityEngine;

namespace Code
{
    public class Die : MonoBehaviour
    {
        [SerializeField] private float m_rollSpeed = 10f;
        private bool m_isRolling = false;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ScenarioManager.OnRoll += ScenarioManager_OnRoll;
        }

        private void ScenarioManager_OnRoll() => m_isRolling = true;

        // Update is called once per frame
        void Update()
        {
            if (m_isRolling)
            {
                transform.Rotate(Time.deltaTime * m_rollSpeed, Time.deltaTime * m_rollSpeed, Time.deltaTime * m_rollSpeed);
            }
        }
    }
}
