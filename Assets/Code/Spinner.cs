using System;
using System.Collections.Generic;
using NUnit.Framework;
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
            slowing
        }

        private void Start()
        {
            ScenarioManager.OnButtonPressed += ScenarioManager_OnButtonPressed;
            ChangeOutcome(Random.Range(0,3));
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
                    SlowSpin(m_inner, m_innerSpinSpeed);
                    break;
                }
                case SpinnerState.fastSpin:
                {
                    FastSpin(m_inner, m_innerSpinSpeed);
                    break;
                }
                case SpinnerState.slowing:
                {
                    break;
                }
            }
            
            switch (OuterSpinnerState)
            {
                case SpinnerState.slowSpin:
                {
                    SlowSpin(m_outer, m_outerSpinSpeed);
                    break;
                }
                case SpinnerState.fastSpin:
                {
                    FastSpin(m_outer, m_outerSpinSpeed);
                    break;
                }
                case SpinnerState.slowing:
                {
                    break;
                }
            }
        }

        private void SlowSpin(Transform spinner, float speed)
        {
            spinner.Rotate(Vector3.left * (speed * Time.deltaTime));
        }

        private void FastSpin(Transform spinner, float speed)
        {
            spinner.Rotate(Vector3.left * (speed * m_fastSpinSpeedFactor * Time.deltaTime));
        }

        public void ChangeState(SpinnerState newState)
        {
            InnerSpinnerState = newState;
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
    }
}
