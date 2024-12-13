using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.Scenarios
{
    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private Button[] m_buttons;
        [SerializeField] private TextMeshProUGUI m_winLossText;
        public static Outcome CurrentOutcome;
        [SerializeField] private List<Outcome> m_availableOutcomes;
        
        
        public delegate void ButtonPressedEvent(int index);
        
        public static event ButtonPressedEvent OnButtonPressed;

        public static event Action OnRoll;

        private void Awake()
        {
            for (int i = 0; i < m_buttons.Length; i++)
            {
                int index = i;
                m_buttons[i].onClick.AddListener(() => OnButtonPressed(index));
            }
            
            OnButtonPressed += ScenarioManager_OnButtonPressed;
            FortuneWheel.OnStopped += FortuneWheel_OnStopped;
        }

        private void Start()
        {
            ChangeOutcome(Random.Range(0,3));
        }

        void HandleButtonPressed(int index) => OnButtonPressed?.Invoke(index);

        public void Roll()
        {
            OnRoll?.Invoke();
            m_winLossText.text = "";
        }
        
        private void ChangeOutcome(int index)
        {
            CurrentOutcome = m_availableOutcomes[index];
        }
        
        private void ScenarioManager_OnButtonPressed(int index)
        {
            ChangeOutcome(index);
        }

        private int m_wheelsStopped = 0;
        
        private void FortuneWheel_OnStopped()
        {
            m_wheelsStopped++;

            if (m_wheelsStopped == 2)
            {
                DeclareWinLossOutcome();
                m_wheelsStopped = 0;
            }
        }

        private void DeclareWinLossOutcome()
        {
            if (CurrentOutcome.Win)
            {
                m_winLossText.text = "WIN!";
            }
            else
            {
                m_winLossText.text = "NO WIN!";
            }
        }
    }
}
