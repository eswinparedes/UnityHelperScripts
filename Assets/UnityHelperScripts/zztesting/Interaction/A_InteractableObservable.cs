using System;
using UniRx;
using UnityEngine;

public abstract class A_InteractableObservable : MonoBehaviour, I_Interactable
{
    [SerializeField] protected InteractionType m_interactionType;

    Subject<I_Interactor> m_interactableObservedStart;
    Subject<I_Interactor> m_interactableObservedEnd;
    Subject<I_Interactor> m_interactableRequestInteractStart;
    Subject<I_Interactor> m_interactablerequestInteractEnd;

    public IObservable<I_Interactor> ObserveStart { get; private set; }
    public IObservable<I_Interactor> ObserveEnd { get; private set; }
    public IObservable<I_Interactor> InteractStart { get; private set; }
    public IObservable<I_Interactor> InteractEnd { get; private set; }
    
    private void Awake()
    {
        m_interactableObservedStart = new Subject<I_Interactor>().AddTo(this);
        m_interactableObservedEnd = new Subject<I_Interactor>().AddTo(this);
        m_interactableRequestInteractStart = new Subject<I_Interactor>().AddTo(this);
        m_interactablerequestInteractEnd = new Subject<I_Interactor>().AddTo(this);

        ObserveStart = m_interactableObservedStart.AsObservable();
        ObserveEnd = m_interactableObservedEnd.AsObservable();
        InteractStart = m_interactableRequestInteractStart.AsObservable();
        InteractEnd = m_interactablerequestInteractEnd.AsObservable();

        OnInitialized();
    }
    public void RequestObserveStart(I_Interactor interactorData)
    {
        Debug.Log("Interactable Observe start  ");
        m_interactableObservedStart.OnNext(interactorData);
    }

    public void RequestObserveEnd(I_Interactor interactorData)
    {
        Debug.Log("Interactable Observe end  ");
        m_interactableObservedEnd.OnNext(interactorData);
    }

    public void RequestInteractStart(I_Interactor interactorData)
    {
        Debug.Log("Interactable interact start  ");
        m_interactableRequestInteractStart.OnNext(interactorData);
    }

    public void RequestInteractEnd(I_Interactor interactorData)
    {
        Debug.Log("Interactable interact end  ");
        m_interactablerequestInteractEnd.OnNext(interactorData);
    }

    public InteractionType InteractionType => m_interactionType;

    public abstract Vector3 Position { get; }
    public abstract void OnInitialized();
}