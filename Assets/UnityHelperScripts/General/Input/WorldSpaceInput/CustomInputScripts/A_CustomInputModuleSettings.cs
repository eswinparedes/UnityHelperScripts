using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public abstract class A_CustomInputModuleSettings : ScriptableObject
{
    public abstract bool PressedCondition { get; set; }
    public abstract bool ReleasedCondition { get; set; }
    public abstract bool SubmitCondition { get; set; }
    public abstract bool CancelCondition { get; set; }

    public abstract Ray InputRay { get; set; }
    public abstract List<RaycastResult> InputRaycastResult { get; set; }
    public abstract float HorizontalAxis { get; set; }
    public abstract float VerticalAxis { get; set; }
    public abstract bool HorizontalAxisThisFrame { get; set; }
    public abstract bool VerticalAxisThisFrame { get; set; }

    public virtual event Action OnInputModuleProcessComplete;
    public virtual event Action OnInputModulePointerDown;
    public virtual event Action OnInputModulePointerUp;

    public virtual event Func<RaycastResult, RaycastResult> GetProcessedResult;

    public virtual void InvokeOnInputModuleProcessComplete() =>
        OnInputModuleProcessComplete?.Invoke();

    public virtual void InvokeOnInputModulePointerDown() =>
        OnInputModulePointerDown?.Invoke();

    public virtual void InvokeOnInputModulePointerUp() =>
        OnInputModulePointerUp?.Invoke();

    public virtual RaycastResult InvokeGetProcessedResult(RaycastResult result) =>
        GetProcessedResult == null ? result : GetProcessedResult(result);
}
