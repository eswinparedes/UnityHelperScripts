using UnityEngine;

namespace SUHScripts
{
    public abstract class SO_A_NoiseData : ScriptableObject
    {
        public abstract INoiseGenerator Generator { get; }
    }
}

