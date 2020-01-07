using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Scriptable Event Basic String")]
public class SO_ScriptableEventBasicString : SO_A_ScriptableEventString
{
    public override event Action<string> OnEventRaised;

    public override void RaiseEvent(string value) =>
        OnEventRaised?.Invoke(value);
}
