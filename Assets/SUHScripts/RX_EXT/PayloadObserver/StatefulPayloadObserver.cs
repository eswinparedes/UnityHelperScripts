using SUHScripts;
using System;

public class PayloadObserverStateSource<T> : IPayloadObserverState<T>
{
    Action<T> m_payloadObservation;
    IState m_state;

    public PayloadObserverStateSource(Action onEnter, Action onExit, Action<float> tick, Action<T> payloadObservation)
    {
        m_payloadObservation = payloadObservation;
        m_state = new ComposableState(onEnter, onExit, tick);
    }

    public PayloadObserverStateSource(IState state, Action<T> payloadObservation)
    {
        m_payloadObservation = payloadObservation;
        m_state = state;
    }

    public void ObservePayload(T payload) => m_payloadObservation(payload);

    public void OnEnter()
    {
        m_state.OnEnter();
    }

    public void OnExit()
    {
        m_state.OnExit();
    }

    public void Tick(float deltaTime)
    {
        m_state.Tick(deltaTime);
    }
}