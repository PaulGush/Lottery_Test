using UnityEngine;

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
        
        [Header("References")] 
        [SerializeField] private Transform m_inner;
        [SerializeField] private Transform m_outer;

        public enum SpinnerState
        {
            idleSpin,
            fastSpin,
            slowing
        }
        
        public SpinnerState SpinState = SpinnerState.idleSpin;
        void Update()
        {
            switch (SpinState)
            {
                case SpinnerState.idleSpin:
                {
                    SlowSpin();
                    break;
                }
                case SpinnerState.fastSpin:
                {
                    FastSpin();
                    break;
                }
                case SpinnerState.slowing:
                {
                    break;
                }
            }
        }

        private void SlowSpin()
        {
            m_inner.Rotate(Vector3.left * (m_innerSpinSpeed * Time.deltaTime));
            m_outer.Rotate(Vector3.left * (m_outerSpinSpeed * Time.deltaTime));
        }

        private void FastSpin()
        {
            m_inner.Rotate(Vector3.left * (m_innerSpinSpeed * m_fastSpinSpeedFactor * Time.deltaTime));
            m_outer.Rotate(Vector3.left * (m_outerSpinSpeed * m_fastSpinSpeedFactor * Time.deltaTime));
        }

        public void ChangeState(SpinnerState newState)
        {
            SpinState = newState;
        }

        public void ChangeState(int newState)
        {
            SpinState = (SpinnerState)newState;
        }
    }
}
