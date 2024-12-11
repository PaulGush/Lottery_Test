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
        
        public SpinnerState InnerSpinnerState = SpinnerState.slowSpin;
        public SpinnerState OuterSpinnerState = SpinnerState.slowSpin;
        
        [Header("References")] 
        [SerializeField] private Transform m_inner;
        [SerializeField] private Transform m_outer;

        public enum SpinnerState
        {
            slowSpin,
            fastSpin,
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
            ChangeOutcome(Random.Range(0,3));
        }

        private void ScenarioManager_OnRoll()
        {
            ChangeInnerState(1);
            ChangeOuterState(1);
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
                case SpinnerState.slowSpin:
                {
                    Spin(m_inner, m_innerSpinSpeed);
                    break;
                }
                case SpinnerState.fastSpin:
                {
                    Spin(m_inner, m_innerSpinSpeed * m_fastSpinSpeedFactor);
                    break;
                }
                case SpinnerState.stopping:
                    LandOnSegment(1);
                    break;
                case SpinnerState.stopped:
                {
                    break;
                }
            }
            
            switch (OuterSpinnerState)
            {
                case SpinnerState.slowSpin:
                {
                    Spin(m_outer, m_outerSpinSpeed);
                    break;
                }
                case SpinnerState.fastSpin:
                {
                    Spin(m_outer, m_outerSpinSpeed * m_fastSpinSpeedFactor);
                    break;
                }
                case SpinnerState.stopping:
                    LandOnSegment(1);
                    break;
                case SpinnerState.stopped:
                {
                    break;
                }
            }
        }

        private void Spin(Transform spinner, float speed)
        {
            spinner.Rotate(Vector3.left * (speed * Time.deltaTime));
        }

        public void ChangeInnerState(int newState)
        {
            InnerSpinnerState = (SpinnerState)newState;
        }
        
        public void ChangeOuterState(int newState)
        {
            OuterSpinnerState = (SpinnerState)newState;
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

        private void LandOnSegment(int segment)
        {
            Spin(m_inner, m_innerSpinSpeed);
            
            float targetRotation = m_innerOriginRotation + (m_innerRotationOffset * (segment - 1));

            Debug.Log("Target segment center: " + targetRotation);
            
            Debug.Log("Target segment extremes: " + (targetRotation - m_innerRotationOffset / 2) + ", " + (targetRotation + m_innerRotationOffset / 2));
            
            Debug.Log(m_inner.eulerAngles.x);
            
            if (m_inner.eulerAngles.x >= (targetRotation - m_innerRotationOffset / 2) &&
                m_inner.eulerAngles.x <= (targetRotation + m_innerRotationOffset / 2))
            {
                Debug.Log("Target segment reached");
                ChangeInnerState(3);
            }

            //m_inner.localRotation = Quaternion.SlerpUnclamped(m_inner.localRotation,
                //Quaternion.Euler(targetRotation, m_inner.localRotation.y, m_inner.localRotation.z), Time.deltaTime * m_innerSpinSpeed);
        }
        
    }
}