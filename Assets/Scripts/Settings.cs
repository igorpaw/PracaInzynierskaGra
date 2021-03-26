using System;
using UnityEngine.Serialization;

namespace Settings
{
    public enum SteeringMethod { Keyboard = 0, Arrows, EyesClosure, EyesPosition, Gesture, Tier, GestCon, MoveAdd, ArrowsAdd }
    public enum SteeringArrowsColor { White = 0, Grey, Yellow, Green, Cyan, Blue, Black, Red }
    public enum Opposite {No = 0, Yes}
    public enum Visible {No = 0, Yes}
    public enum LostChance {No = 0, Yes}

        [Serializable]
        public class Settings
        {
            public SteeringMethod steeringMethod;
            
            public Visible showArrows;
            public SteeringArrowsColor steeringArrowsColor;
            public Opposite opposite;
            [FormerlySerializedAs("lostLives")] public LostChance lostChance;
        }
}

