using System;
using UnityEngine;
using UnityEngine.Events;

public interface I_Interactor
{
    TransformData InteractorData { get; }
}

public interface I_Interactable
{
    void RequestObserveStart(I_Interactor interactorData);
    void RequestObserveEnd(I_Interactor interactorData);
    void RequestInteractStart(I_Interactor interactorData);
    void RequestInteractEnd(I_Interactor interactorData);

    Vector3 Position { get; }
    InteractionType InteractionType { get; }
}

public enum InteractionType
{
    Basic,
    LockView
}

[Serializable]
public class UnityEvent_Interactor : UnityEvent<I_Interactor>
{

}
