using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scenarios
{
    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private Button[] m_buttons;
        
        public delegate void ButtonPressedEvent(int index);
        
        public static event ButtonPressedEvent OnButtonPressed;

        public static event Action OnRoll;
        public static event Action OnLand;

        private void Awake()
        {
            for (int i = 0; i < m_buttons.Length; i++)
            {
                int index = i;
                m_buttons[i].onClick.AddListener(() => OnButtonPressed(index));
            }
        }
        
        void HandleButtonPressed(int index) => OnButtonPressed?.Invoke(index);

        public void Roll()
        {
            OnRoll?.Invoke();
        }

        public void Land()
        {
            OnLand?.Invoke();
        }
    }
}
