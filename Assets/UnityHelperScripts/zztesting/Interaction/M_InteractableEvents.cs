using UnityEngine;
using UniRx;

public class M_InteractableEvents : A_InteractableObservable
{
    [SerializeField] protected Transform m_root;
    [SerializeField] protected UnityEvent_Interactor m_onInteractableObservedStart;
    [SerializeField] protected UnityEvent_Interactor m_onInteractableObservedEnd;
    [SerializeField] protected UnityEvent_Interactor m_onInteractableRequestInteractStart;
    [SerializeField] protected UnityEvent_Interactor m_onInteractablerequestInteractEnd;

    public override Vector3 Position => m_root.position;

    public override void OnInitialized()
    {
        ObserveStart.Subscribe(m_onInteractableObservedStart).AddTo(this);
        ObserveEnd.Subscribe(m_onInteractableObservedEnd).AddTo(this);
        InteractStart.Subscribe(m_onInteractableRequestInteractStart).AddTo(this);
        InteractEnd.Subscribe(m_onInteractablerequestInteractEnd).AddTo(this);
    }
}
