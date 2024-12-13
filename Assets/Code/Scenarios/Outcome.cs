using UnityEngine;

namespace Code.Scenarios
{
    [CreateAssetMenu(fileName = "New Outcome", menuName = "Lottery Outcome")]
    public class Outcome : ScriptableObject
    {
        public int Die1;
        public int Die2;
        public AnimationClip[] DiceAnimationClips;
        public int InnerSpinner;
        public int OuterSpinner;
        public bool Win = false;
        
    }
}
