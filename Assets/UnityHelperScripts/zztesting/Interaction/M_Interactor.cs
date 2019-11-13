using UnityEngine;
using UniRx;
using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;
using static SUHScripts.Functional.PhysicsCasting;
using System;
using Unit = UniRx.Unit;

public class M_Interactor : MonoBehaviour, I_Interactor
{
    [SerializeField] FPSRoot m_root = default;
    [SerializeField] float m_interactDistance = default;
    [SerializeField] LayerMask m_interactableLayerMask = default;

    Subject<InteractionType> m_interactableStarted;
    Subject<Unit> m_interactableEnded;

    public IObservable<InteractionType> InteractableStarted { get; private set; }
    public IObservable<Unit> InteractableEnded { get; private set; }

    public TransformData InteractorData => m_root.FPSCamera.Camera.transform.ExtractData();
    
    private void Awake()
    {
        m_interactableStarted = new Subject<InteractionType>().AddTo(this);
        m_interactableEnded = new Subject<Unit>().AddTo(this);
        InteractableStarted = m_interactableStarted.AsObservable();
        InteractableEnded = m_interactableEnded.AsObservable();
    }
    private void Start()
    {
        //Handle interaction focus
        Option<I_Interactable> interactableFocused = NONE;

        //SUHS TODO: Create a raycast combinator that a raycastLooksfor and behind the seens
        m_root
            .OnUpdate
            .Select(tick => Raycast(m_root.FPSCamera.Camera.transform, range : m_interactDistance, mask : m_interactableLayerMask))
            .Select(result => result.QueryComponentOption<I_Interactable>())
            .Subscribe(interactableOption =>
            {
                interactableFocused =
                    interactableFocused
                    .ForNewEntryUpdate(
                        interactableOption,
                        val => val.RequestObserveStart(this),
                        val => val.RequestObserveEnd(this));
            })
            .AddTo(this);

        //Handle Interactable Selection
        Option<I_Interactable> interactableSelected = NONE;

        m_root
            .Inputs
            .OnInteractStart
            .Where(_ => !interactableSelected.IsSome && interactableFocused.IsSome)
            .Subscribe(_ =>
            {
                interactableSelected = interactableFocused;
                interactableSelected.Value.RequestInteractStart(this);
                m_interactableStarted.OnNext(interactableSelected.Value.InteractionType);
            })
            .AddTo(this);

        m_root
            .Inputs
            .OnInteractEnd
            .Where(_ => interactableSelected.IsSome)
            .Subscribe(_ =>
            {
                interactableSelected.Value.RequestInteractEnd(this);
                interactableSelected = NONE;
                m_interactableEnded.OnNext(Unit.Default);
            })
            .AddTo(this);

        return;
    }
}
