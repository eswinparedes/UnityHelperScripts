using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_A_NoiseData : ScriptableObject
{
    public abstract INoiseGenerator Generator { get; }
}
