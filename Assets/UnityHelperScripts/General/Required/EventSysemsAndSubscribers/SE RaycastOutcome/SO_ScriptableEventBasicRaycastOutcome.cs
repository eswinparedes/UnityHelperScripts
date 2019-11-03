using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Scriptable Event Basic RaycastOutcome")]
public class SO_ScriptableEventBasicRaycastOutcome : SO_A_ScriptableEventRaycastOutcome
{
    public override event Action<RaycastOutcome> OnEventRaised;

    public override void RaiseEvent(RaycastOutcome outcome) =>
        OnEventRaised?.Invoke(outcome);
}
