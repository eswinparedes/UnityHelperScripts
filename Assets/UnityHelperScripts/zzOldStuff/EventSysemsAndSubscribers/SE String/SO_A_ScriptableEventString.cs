using UnityEngine;
using System;

public abstract class SO_A_ScriptableEventString : ScriptableObject
{
    public abstract event Action<String> OnEventRaised;
    public abstract void RaiseEvent(string value);
}
