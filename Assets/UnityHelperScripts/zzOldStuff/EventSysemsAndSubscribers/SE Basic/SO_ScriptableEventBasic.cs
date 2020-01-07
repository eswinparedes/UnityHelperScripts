using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/Scriptable Event")]
public class SO_ScriptableEventBasic : SO_A_ScriptableEvent
{
    public override event Action OnEventRaised;
    public override void RaiseEvent() => OnEventRaised?.Invoke();
}
