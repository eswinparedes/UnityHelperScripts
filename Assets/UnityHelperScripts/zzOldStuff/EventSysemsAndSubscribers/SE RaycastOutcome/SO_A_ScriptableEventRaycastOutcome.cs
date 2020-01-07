using UnityEngine;
using System;

public abstract class SO_A_ScriptableEventRaycastOutcome : ScriptableObject
{
    public abstract event Action<RaycastOutcome> OnEventRaised;
    public abstract void RaiseEvent(RaycastOutcome outcome);
}
