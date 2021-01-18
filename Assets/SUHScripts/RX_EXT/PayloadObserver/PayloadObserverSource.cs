using System;
using SUHScripts;

public class PayloadObserverSource<T> : IPayloadObserver<T>
{
    Action<T> m_payloadObservation;

    public PayloadObserverSource(Action<T> payloadObservation)
    {
        m_payloadObservation = payloadObservation;
    }

    public void ObservePayload(T payload) => m_payloadObservation(payload);
}