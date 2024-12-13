using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scenarios
{
    [CreateAssetMenu(fileName = "New Outcome", menuName = "Lottery Outcome")]
    public class Outcome : ScriptableObject
    {
        public AnimationClip[] DiceAnimationClips;
        [FormerlySerializedAs("InnerSpinner")] public int InnerWheel;
        [FormerlySerializedAs("OuterSpinner")] public int OuterWheel;
        public bool Win = false;
        
    }
}
