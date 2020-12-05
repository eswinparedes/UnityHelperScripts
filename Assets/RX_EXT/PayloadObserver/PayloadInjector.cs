using System.Collections.Generic;
using SUHScripts;
using UniRx;
using System;
using UnityEngine;

namespace SUHScripts
{
    public class PayloadInjector<T> : IValvable
    {
        public PayloadInjector(IReadOnlyList<GameObject> obervePayloadsOn, IReadOnlyList<IPayloadObserver<T>> injectPayloadsTo, Func<GameObject, IObservable<T>> observableSourceFunc, bool valveStartOn)
        {
            if (obervePayloadsOn.Count == 0)
            {
                Debug.LogError("No components available to observer");
                return;
            }

            SetValve(valveStartOn);

            m_observers = new List<IPayloadObserver<T>>(injectPayloadsTo);

            var pldStreams = new List<IObservable<T>>();

            for (int i = 0; i < obervePayloadsOn.Count; i++)
            {
                var go = obervePayloadsOn[i];
                var pldObservable = observableSourceFunc(go);

                pldStreams.Add(pldObservable);
            }

            var pld = pldStreams.Merge().Where(_ => m_isOn).Do(t =>
            {
                for (int i = 0; i < m_observers.Count; i++)
                {
                    m_observers[i].ObservePayload(t);
                }
            }).Publish();

            pld.Connect();
            PayloadStream = pld;
        }

        List<IPayloadObserver<T>> m_observers;
        bool m_isOn = false;

        public void PushObserver(IPayloadObserver<T> observer)
        {
            m_observers.Add(observer);
        }

        public void SetValve(bool isOn)
        {
            m_isOn = isOn;
        }

        public IObservable<T> PayloadStream { get; }
    }
}