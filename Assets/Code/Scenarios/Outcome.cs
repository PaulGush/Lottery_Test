using UnityEngine;

namespace Code.Scenarios
{
    [CreateAssetMenu(fileName = "New Outcome", menuName = "Lottery Outcome")]
    public class Outcome : ScriptableObject
    {
        public AnimationClip[] DiceAnimationClips;
        public int InnerSpinner;
        public int OuterSpinner;
        public bool Win = false;
        
    }
}
