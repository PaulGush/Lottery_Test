using System;
using Code.Scenarios;
using UnityEngine;
using Random = UnityEngine.Random;

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
            m_animator.Play(ScenarioManager.CurrentOutcome.DiceAnimationClips[Random.Range(0, ScenarioManager.CurrentOutcome.DiceAnimationClips.Length)].name);
        }

        public void DiceLanded()
        {
            OnDiceLanded?.Invoke();
        }
    }
}
