using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private Button[] m_buttons;
        
        public delegate void ButtonPressedEvent(int index);
        
        public static event ButtonPressedEvent OnButtonPressed;

        private void Awake()
        {
            for (int i = 0; i < m_buttons.Length; i++)
            {
                int index = i;
                m_buttons[i].onClick.AddListener(() => OnButtonPressed(index));
            }
        }
        
        void HandleButtonPressed(int index) => OnButtonPressed?.Invoke(index);
    }
}
