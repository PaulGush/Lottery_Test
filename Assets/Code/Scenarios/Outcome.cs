using UnityEngine;

namespace Code
{
    [CreateAssetMenu(fileName = "New Outcome", menuName = "Lottery Outcome")]
    public class Outcome : ScriptableObject
    {
        public int InnerSpinner;
        public int OuterSpinner;
        public int Die1;
        public int Die2;
        public bool Win = false;
    }
}
