using UnityEngine;
using UniRx;
using System;

namespace SUHScripts
{
    public abstract class PayloadObserverAttachment<T> : MonoBehaviour, IPayloadObserver<T>
    {
        Subject<T> m_payloadStreamSubject = new Subject<T>();
        public IObservable<T> PayloadStream => m_payloadStreamSubject;
        public virtual T Transformer(T input)
        {
            return input;
        }
        public void ObservePayload(T payLoad)
        {
            m_payloadStreamSubject.OnNext(payLoad);
        }
    }
}