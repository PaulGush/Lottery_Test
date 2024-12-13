using Code.Scenarios;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class FortuneWheel : MonoBehaviour
    {
        [SerializeField] private Transform m_fortuneWheel;
        
        [Header("Values")]
        [SerializeField] private int m_segments = 6;

        [SerializeField] private bool m_isOuterWheel;
        
        [Header("Speed")]
        [SerializeField] private float m_fastRotationSpeed = 100f;
        [SerializeField] private float m_slowRotationSpeed = 40f;
        [SerializeField] private float m_decelerationRate = 5f;
        [SerializeField] private float m_currentRotationSpeed;
        
        [SerializeField] private float m_targetAngle = 0f;
        [SerializeField] private float m_currentAngle = 0f;
        
        [SerializeField] private bool m_isSpinning = false;
        [SerializeField] private bool m_isStopping = false;

        private float m_originRotation;
        
        private float m_rotationOffset;

        private void Awake()
        {
            InitializeOffsets();
        }

        private void InitializeOffsets()
        {
            m_originRotation = m_fortuneWheel.rotation.eulerAngles.x;
            m_rotationOffset = 360f / m_segments;
        }
        
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

            if (m_isOuterWheel)
            {
                m_targetAngle = m_originRotation + ((ScenarioManager.CurrentOutcome.OuterSpinner - 1) * m_rotationOffset);
            }
            else
            {
                m_targetAngle = m_originRotation + ((ScenarioManager.CurrentOutcome.InnerSpinner - 1) * m_rotationOffset);
            }

            
        }

        private void RequestStartSpin()
        {
            m_isSpinning = true;
            m_currentRotationSpeed = m_fastRotationSpeed;
        }

        private void RequestStopSpin()
        {
            m_isStopping = true;
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

        private void Decelerate()
        {
            m_currentRotationSpeed -= m_decelerationRate * Time.deltaTime;
            m_currentRotationSpeed = Mathf.Max(m_slowRotationSpeed, m_currentRotationSpeed);
            
            if (m_currentRotationSpeed <= m_slowRotationSpeed)
            {
                m_isSpinning = false;
                StopWheel();
            }
        }

        private void StopWheel()
        {
            m_isStopping = true;
            
            float angleDiff = Mathf.DeltaAngle(m_currentAngle, m_targetAngle);
            if (angleDiff > 180) {
                angleDiff -= 360;
            }
            float rotationAmount = Mathf.Min(m_currentRotationSpeed * Time.deltaTime, Mathf.Abs(angleDiff));

            m_currentAngle += rotationAmount;
            m_fortuneWheel.transform.localRotation = Quaternion.Euler(m_currentAngle, 0, 0);

            if (Mathf.Abs(angleDiff) < 0.1f)
            {
                m_currentAngle = m_targetAngle;
                m_isStopping = false;
            }
        }
        
        private void HandleSpin()
        {
            if (m_isSpinning)
            {
                m_currentAngle += m_currentRotationSpeed * Time.deltaTime;
                m_fortuneWheel.transform.localRotation = Quaternion.Euler(m_currentAngle, 0, 0);
            }
        }
    }
}