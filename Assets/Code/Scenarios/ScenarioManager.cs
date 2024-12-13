using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.Scenarios
{
    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private Button[] m_buttons;
        public static Outcome CurrentOutcome;
        [SerializeField] private List<Outcome> m_availableOutcomes;
        
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
            
            OnButtonPressed += ScenarioManager_OnButtonPressed;
        }

        private void Start()
        {
            ChangeOutcome(Random.Range(0,3));
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
        
        private void ChangeOutcome(int index)
        {
            CurrentOutcome = m_availableOutcomes[index];
        }
        
        private void ScenarioManager_OnButtonPressed(int index)
        {
            ChangeOutcome(index);
        }
    }
}
