using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scenarios
{
    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private Button[] m_buttons;
        [SerializeField] private TextMeshProUGUI m_winLossText;
        [SerializeField] private TextMeshProUGUI m_spinsRemainingText;
        public static Outcome CurrentOutcome;
        private int m_currentOutcomeIndex = 0;
        [SerializeField] private List<Outcome> m_availableOutcomes;
        [SerializeField] private bool m_isRolling = false;
        [SerializeField] private int m_spinsRemaining = 3;
        
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
            ChangeOutcome(m_currentOutcomeIndex);
            UpdateSpinsRemaining(false);
        }

        void HandleButtonPressed(int index) => OnButtonPressed?.Invoke(index);

        public void Roll()
        {
            if (!m_isRolling && m_spinsRemaining > 0)
            {
                OnRoll?.Invoke();
                m_isRolling = true;
                
                UpdateSpinsRemaining();
                
                m_winLossText.text = ""; //So the text is clear when doing next roll (IE You don't still have NO WIN on screen when it's mid-roll)
            }
        }

        private void UpdateSpinsRemaining(bool decrement = true)
        {
            if (decrement)
            {
                m_spinsRemaining--;
            }
            
            m_spinsRemainingText.text = "SPINS LEFT: " + m_spinsRemaining.ToString();
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
            m_winLossText.text = CurrentOutcome.Win ? "WIN £100!" : "NO WIN!";

            IncrementOutcome();
        }

        private void IncrementOutcome()
        {
            m_currentOutcomeIndex++;

            if (m_currentOutcomeIndex >= m_availableOutcomes.Count)
            {
                m_currentOutcomeIndex = 0;
            }

            ChangeOutcome(m_currentOutcomeIndex);
            
            m_isRolling = false;
        }
    }
}
