using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SUHScripts.Functional;
using UniRx;
using UnityEngine.Rendering.PostProcessing;

namespace SUHScripts
{
    /// <summary>
    /// Represents an action that may or may not happen, true means it did, false means it did not
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate bool MayHappen<T>(T value);
    public delegate bool MayHappen<T, U>(T tVal, U uVal);

    [System.Serializable]
    public abstract class InspectorKeyValuePair<TKey, TValue>
    {
        [SerializeField] TKey m_key = default;
        [SerializeField] TValue m_value = default;
        public TKey Key => m_key;
        public TValue Value => m_value;
    }


    public interface IPayloadObserver<TPayload>
    {
        void ObservePayload(TPayload payLoad);
    }


   


    public static partial class GenericHelpers 
    {
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
        public static int InjectPayloadToObserver<Tpayload> (Tpayload payload, GameObject dependencySearchable)
        {
            var comps = dependencySearchable.GetComponents<IPayloadObserver<Tpayload>>();
            int idx = 0;
            for(int i = 0; i < comps.Length; i++)
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

        public static Option<IObservable<U>> TryGetMergedObservables<T, U>(GameObject target, Func<T, IObservable<U>> targetFunction)
        {
            var comps = target.GetComponents<T>();

            if(comps != null && comps.Length > 0)
            {
                var obvss = new List<IObservable<U>>();
                for(int i =0; i < comps.Length; i++)
                {
                    var obv = targetFunction(comps[i]);
                    obvss.Add(obv);
                }

                return obvss.Merge().AsOption_SAFE();
            }
            else
            {
                return None.Default;
            }
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyList<InspectorKeyValuePair<TKey, TValue>> @this)
        {
            var d = new Dictionary<TKey, TValue>();

            for(int i =0; i < @this.Count; i++)
            {
                d.Add(@this[i].Key, @this[i].Value);
            }

            return d;
        }
        public static Dictionary<TKey, Action<TKey, TValue>> ActionLock<TKey, TValue>(this Dictionary<TKey, TValue> @this, Action<TKey, TValue> action)
        {
            var d = new Dictionary<TKey, Action<TKey, TValue>>();

            foreach(var kvp in @this)
            {
                Action<TKey, TValue> act =
                    (key, value) =>
                    {
                        if (@this.ContainsKey(key))
                            action(key, value);
                    };

                d.Add(kvp.Key, act);
            }

            return d;
        }


        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this IReadOnlyList<Dictionary<TKey, TValue>> @this)
        {
            var d = new Dictionary<TKey, TValue>();

            for(int i =0;i < @this.Count; i++)
            {
                foreach(var kvp in @this[i])
                {
                    d.Add(kvp.Key, kvp.Value);
                }
            }

            return d;
        }

        public static Option<T> TryGetSettings<T>(this PostProcessVolume volume) where T : PostProcessEffectSettings
        {
            T t;
            volume.profile.TryGetSettings(out t);

            return t.AsOption_SAFE();
        }

        public static Action Append(this Action action, Action toAppend) =>
            () =>
            {
                action();
                toAppend();
            };
      
    }

    
}

