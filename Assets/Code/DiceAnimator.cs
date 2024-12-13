using System;
using Code.Scenarios;
using UnityEngine;

namespace Code
{
    public class DiceAnimator : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;

        public static event Action OnDiceLanded;
        private void Start()
        {
            ScenarioManager.OnRoll += ScenarioManager_OnRoll;
        }

        private void ScenarioManager_OnRoll()
        {
            m_animator.Play(ScenarioManager.CurrentOutcome.DiceAnimationClips[0].name);
        }

        public void DiceLanded()
        {
            OnDiceLanded?.Invoke();
        }
    }
}
