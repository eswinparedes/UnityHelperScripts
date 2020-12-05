using System.Collections;
using SUHScripts;
using UniRx;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public static class PayloadObserver
    {
        public static IDisposable SubscribePayloadObservers<T>(this IObservable<T> @this, params IPayloadObserver<T>[] payloadObservers)
        {
            return @this.Subscribe(t =>
            {
                for (int i = 0; i < payloadObservers.Length; i++)
                {
                    payloadObservers[i].ObservePayload(t);
                }
            });
        }

        public static int InjectPayloadToObserver<Tpayload>(Tpayload payload, Component dependencySearchable)
        {
            var comps = dependencySearchable.GetComponents<IPayloadObserver<Tpayload>>();
            int idx = 0;
            for (int i = 0; i < comps.Length; i++)
            {
                idx++;
                comps[i].ObservePayload(payload);
            }

            return idx;
        }
        public static int InjectPayloadToObserver<Tpayload>(Tpayload payload, GameObject dependencySearchable)
        {
            var comps = dependencySearchable.GetComponents<IPayloadObserver<Tpayload>>();
            int idx = 0;
            for (int i = 0; i < comps.Length; i++)
            {
                idx++;
                comps[i].ObservePayload(payload);
            }

            return idx;
        }

        public static int InjectPayloadToObservers<TPayload>(TPayload payload, params Component[] dependencySearchables)
        {
            var comps = new List<IPayloadObserver<TPayload>[]>();
            var idx = 0;

            for (int i = 0; i < dependencySearchables.Length; i++)
            {
                var cs = dependencySearchables[i].GetComponents<IPayloadObserver<TPayload>>();

                if (cs.Length == 0)
                    Debug.LogWarning($"No Interactor Observers found on {dependencySearchables[i]}");
                else
                    comps.Add(cs);
            }


            for (int i = 0; i < comps.Count; i++)
            {
                for (int j = 0; j < comps[i].Length; j++)
                {
                    idx++;
                    comps[i][j].ObservePayload(payload);
                }
            }

            return idx;
        }
        public static int InjectPayloadToObservers<TPayload>(TPayload payload, params GameObject[] dependencySearchables)
        {
            var comps = new List<IPayloadObserver<TPayload>[]>();
            var idx = 0;

            for (int i = 0; i < dependencySearchables.Length; i++)
            {
                var cs = dependencySearchables[i].GetComponents<IPayloadObserver<TPayload>>();

                if (cs.Length == 0)
                    Debug.LogWarning($"No Payload Observers found on {dependencySearchables[i]}");
                else
                    comps.Add(cs);
            }


            for (int i = 0; i < comps.Count; i++)
            {
                for (int j = 0; j < comps[i].Length; j++)
                {
                    idx++;
                    comps[i][j].ObservePayload(payload);
                }
            }

            return idx;
        }
    }
}