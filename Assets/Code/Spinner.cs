using System.Collections.Generic;
using Code.Scenarios;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public class Spinner : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField] private int m_innerSegments = 6;
        [SerializeField] private int m_outerSegments = 6;
        [SerializeField] private float m_innerSpinSpeed = 5f;
        [SerializeField] private float m_outerSpinSpeed = 5f;
        [SerializeField] private float m_fastSpinSpeedFactor = 5f;
        [SerializeField] private Outcome m_currentOutcome;
        [SerializeField] private List<Outcome> m_availableOutcomes;
        [SerializeField] private List<float> m_rotationTargets;
        
        public SpinnerState InnerSpinnerState = SpinnerState.slow;
        public SpinnerState OuterSpinnerState = SpinnerState.slow;
        
        [Header("References")] 
        [SerializeField] private Transform m_inner;
        [SerializeField] private Transform m_outer;

        public enum SpinnerState
        {
            slow,
            fast,
            stopping,
            stopped
        }

        private void Awake()
        {
            InitializeSpinners();
        }

        private void Start()
        {
            ScenarioManager.OnButtonPressed += ScenarioManager_OnButtonPressed;
            ScenarioManager.OnRoll += ScenarioManager_OnRoll;
            ScenarioManager.OnLand += ScenarioManager_OnLand;
            
            ChangeOutcome(Random.Range(0,3));
        }

        private void ScenarioManager_OnRoll()
        {
            InnerSpinnerState = SpinnerState.fast;
            OuterSpinnerState = SpinnerState.fast;
        }

        private void ScenarioManager_OnLand()
        {
            if (InnerSpinnerState != SpinnerState.stopped)
            {
                InnerSpinnerState = SpinnerState.stopping;
            }

            if (OuterSpinnerState != SpinnerState.stopped)
            {
                OuterSpinnerState = SpinnerState.stopping;
            }
        }

        private void ScenarioManager_OnButtonPressed(int index)
        {
            ChangeOutcome(index);
        }

        void Update()
        {
            HandleSpinnerStates();

        }

        private void HandleSpinnerStates()
        {
            switch (InnerSpinnerState)
            {
                case SpinnerState.slow:
                {
                    Spin(m_inner, m_innerSpinSpeed);
                    break;
                }
                case SpinnerState.fast:
                {
                    Spin(m_inner, m_innerSpinSpeed * m_fastSpinSpeedFactor);
                    break;
                }
                case SpinnerState.stopping:
                    LandOnSegment(m_currentOutcome.InnerSpinner, m_inner, m_innerOriginRotation, m_innerRotationOffset, m_innerSpinSpeed);
                    break;
                case SpinnerState.stopped:
                {
                    break;
                }
            }
            
            switch (OuterSpinnerState)
            {
                case SpinnerState.slow:
                {
                    Spin(m_outer, m_outerSpinSpeed);
                    break;
                }
                case SpinnerState.fast:
                {
                    Spin(m_outer, m_outerSpinSpeed * m_fastSpinSpeedFactor);
                    break;
                }
                case SpinnerState.stopping:
                    LandOnSegment(m_currentOutcome.OuterSpinner, m_outer, m_outerOriginRotation, m_outerRotationOffset, m_outerSpinSpeed);
                    break;
                case SpinnerState.stopped:
                {
                    break;
                }
            }
        }

        private void Spin(Transform spinner, float speed)
        {
            spinner.Rotate(Vector3.right * (speed * Time.deltaTime));
        }

        private void ChangeOutcome(int index)
        {
            m_currentOutcome = m_availableOutcomes[index];
        }

        private float m_innerOriginRotation;
        private float m_outerOriginRotation;
        private float m_innerRotationOffset;
        private float m_outerRotationOffset;
        
        private void InitializeSpinners()
        {
            m_innerOriginRotation = m_inner.rotation.eulerAngles.x;
            m_outerOriginRotation = m_outer.rotation.eulerAngles.x;
            
            m_innerRotationOffset = 2f / m_innerSegments;
            m_outerRotationOffset = 2f / m_outerSegments;
        }

        private void LandOnSegment(int segment, Transform spinner, float originRotation, float rotationOffset, float spinSpeed)
        {
            float targetRotation = m_rotationTargets[segment - 1];

            Spin(spinner, spinSpeed * m_fastSpinSpeedFactor);

            if (segment < 4)
            {
                if (spinner.eulerAngles.x >= targetRotation - rotationOffset / 2 &&
                    spinner.eulerAngles.x <= (targetRotation + rotationOffset / 2))
                {
                    Debug.LogError("FOUND TARGET!");
                }
            }
            else
            {
                
            }

            if (spinner.rotation.x >= (targetRotation - rotationOffset / 2) &&
                spinner.rotation.x <= (targetRotation + rotationOffset / 2))
            {
                spinner.localRotation = new Quaternion(targetRotation, spinner.localRotation.y, spinner.localRotation.z, 0);
                    
                if (spinner.name == "inner_spinner")
                {
                    InnerSpinnerState = SpinnerState.stopped;
                }
                else
                {
                    OuterSpinnerState = SpinnerState.stopped;
                }
            }
        }
    }
}