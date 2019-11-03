using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

public static class UNIRX_Ext 
{
    public static IDisposable Subscribe<T>(this IObservable<T> @this, UnityEvent unityEvent) =>
        @this.Subscribe(x => unityEvent.Invoke());

    public static IDisposable Subscribe<T>(this IObservable<T> @this, UnityEvent<T> unityEvent)=>
        @this.Subscribe(unityEvent.Invoke);

    public static IDisposable Subscribe<T>(this IObservable<T> @this, Action action) =>
        @this.Subscribe(x => action());

    public static IDisposable SubscribeOnce<T>(this IObservable<T> @this, UnityEvent unityEvent) =>
        @this
        .Take(1)
        .Subscribe(unityEvent);

    public static IDisposable SubscribeOnce<T>(this IObservable<T> @this, UnityEvent<T> unityEvent) =>
        @this
        .Take(1)
        .Subscribe(unityEvent);

    public static IDisposable SubscribeOnce<T>(this IObservable<T> @this, Action<T> action) =>
        @this
        .Take(1)
        .Subscribe(action);

    public static IDisposable SubscribeOnce<T>(this IObservable<T> @this, Action action) =>
        @this
        .Take(1)
        .Subscribe(action);

    public static IDisposable AsDisposable(this List<IDisposable> @this) =>
        Disposable.Create(() =>
        {
            for (int i = 0; i < @this.Count; i++)
                @this[i].Dispose();

            @this.Clear();
        });

    public static IObservable<T> DoLog<T>(this IObservable<T> @this, string message) =>
        @this.Do(value => Debug.Log($"{message} : {value}"));

    public static IDisposable SubscribeCatched<T>(this IObservable<T> @this, Action<T> action) =>
        @this
        .Subscribe(
            action, 
            onError: e => Debug.LogError($"Exception observed: {e.Message}"));

    public static IDisposable AddTo(this IDisposable @this, List<IDisposable> subList)
    {
        subList.Add(@this);
        return @this; 
    }
}