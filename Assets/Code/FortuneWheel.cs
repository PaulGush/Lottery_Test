using Code.Scenarios;
using UnityEngine;

namespace Code
{
    public class FortuneWheel : MonoBehaviour
    {
        [SerializeField] private Transform fortuneWheel;
        
        [SerializeField] private float fastRotationSpeed = 100f;
        [SerializeField] private float slowRotationSpeed = 40f;
        [SerializeField] private float decelerationRate = 5f;
        [SerializeField] private float m_currentRotationSpeed;
        
        [SerializeField] private float targetAngle = 0f;
        [SerializeField] private float currentAngle = 0f;
        
        [SerializeField] private bool m_isSpinning = false;
        [SerializeField] private bool m_isStopping = false;
        

        private void Start()
        {
            ScenarioManager.OnRoll += ScenarioManager_OnRoll;
            ScenarioManager.OnLand += ScenarioManager_OnLand;
        }

        private void ScenarioManager_OnRoll()
        {
            RequestStartSpin();
        }

        private void ScenarioManager_OnLand()
        {
            RequestStopSpin();
            targetAngle = 270;
        }

        private void RequestStartSpin()
        {
            m_isSpinning = true;
            m_currentRotationSpeed = fastRotationSpeed;
        }

        private void RequestStopSpin()
        {
            m_isStopping = true;
        }

        private void Decelerate()
        {
            m_currentRotationSpeed -= decelerationRate * Time.deltaTime;
            m_currentRotationSpeed = Mathf.Max(slowRotationSpeed, m_currentRotationSpeed);
            
            if (m_currentRotationSpeed <= slowRotationSpeed)
            {
                m_isSpinning = false;
                StopWheel();
            }
        }

        void Update()
        {
            HandleDeceleration();
            HandleSpin();
        }

        private void HandleDeceleration()
        {
            if (m_isStopping)
            {
                Decelerate();
            }
        }

        private void HandleSpin()
        {
            if (m_isSpinning)
            {
                currentAngle += m_currentRotationSpeed * Time.deltaTime;
                fortuneWheel.transform.localRotation = Quaternion.Euler(currentAngle, 0, 0);
            }
        }

        private void StopWheel()
        {
            m_isStopping = true;
            
            float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
            if (angleDiff > 180) {
                angleDiff -= 360;
            }
            float rotationAmount = Mathf.Min(m_currentRotationSpeed * Time.deltaTime, Mathf.Abs(angleDiff));

            currentAngle += rotationAmount;
            fortuneWheel.transform.localRotation = Quaternion.Euler(currentAngle, 0, 0);

            if (Mathf.Abs(angleDiff) < 0.1f)
            {
                currentAngle = targetAngle;
                m_isStopping = false;
            }
        }
    }
}