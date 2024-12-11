using System;
using System.Collections;
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
            InnerSpinnerState = SpinnerState.stopping;
            OuterSpinnerState = SpinnerState.stopping;
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
                    LandOnSegment(1, m_inner, m_innerOriginRotation, m_innerRotationOffset,m_innerSpinSpeed, out InnerSpinnerState);
                    break;
                case SpinnerState.stopped:
                {
                    Debug.Log("Inner Spinner Stopped");
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
                    LandOnSegment(1, m_outer, m_outerOriginRotation, m_outerRotationOffset,m_outerSpinSpeed, out OuterSpinnerState);
                    break;
                case SpinnerState.stopped:
                {
                    Debug.Log("Outer Spinner Stopped");
                    break;
                }
            }
        }

        private void Spin(Transform spinner, float speed)
        {
            spinner.Rotate(Vector3.left * (speed * Time.deltaTime));
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
            
            m_innerRotationOffset = 360f / m_innerSegments;
            m_outerRotationOffset = 360f / m_outerSegments;
        }

        private void LandOnSegment(int segment, Transform spinner, float originRotation, float rotationOffset, float spinSpeed, out SpinnerState spinnerState)
        {
            Spin(spinner, spinSpeed);
            
            float targetRotation = originRotation + (rotationOffset * (segment - 1));

            Debug.Log("Target segment center: " + targetRotation);
            
            Debug.Log("Target segment extremes: " + (targetRotation - rotationOffset / 2) + ", " + (targetRotation + rotationOffset / 2));
            
            Debug.Log(spinner.eulerAngles.x);
            
            if (spinner.eulerAngles.x >= (targetRotation - rotationOffset / 2) &&
                spinner.eulerAngles.x <= (targetRotation + rotationOffset / 2))
            {
                Debug.Log("Target segment reached");
                spinnerState = SpinnerState.stopped;
                return;
            }

            spinnerState = SpinnerState.stopping;
        }
        
    }
}