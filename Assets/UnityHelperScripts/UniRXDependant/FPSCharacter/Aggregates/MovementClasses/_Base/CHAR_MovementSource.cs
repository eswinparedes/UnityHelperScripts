using System;
using UnityEngine;

public abstract class CHAR_MovementSource : ScriptableObject
{
    public abstract CHAR_MovementInstance Build
        (FPSRoot root);
}
