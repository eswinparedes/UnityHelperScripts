using UnityEngine;
using System;

public abstract class SO_A_ScriptableEvent : ScriptableObject
{
    public abstract event Action OnEventRaised;
    public abstract void RaiseEvent();
}
