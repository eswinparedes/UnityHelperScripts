using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_A_NoiseData : ScriptableObject, I_NoiseGeneratorData
{
    public abstract I_NoiseGeneratorVector3 GetGenerator();
}
