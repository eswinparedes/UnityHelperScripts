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

    public static IDisposable Subscribe_EXLOG<T>(this IObservable<T> @this, Action<T> onNext, Action onComplete) =>
        @this
        .Subscribe(
            onNext, 
            onCompleted: () => onComplete.Invoke(),
            onError: e => Debug.LogError($"Exception observed: {e.Message}"));

    public static IDisposable Subscribe_EXLOG<T>(this IObservable<T> @this, Action<T> onNext) =>
        @this
        .Subscribe(
            onNext,
            onError: e => Debug.LogError($"Exception observed: {e.Message}"));

    public static IDisposable AddTo(this IDisposable @this, List<IDisposable> subList)
    {
        subList.Add(@this);
        return @this; 
    }

    public static IDisposable SubscribeDebug<T>(this IObservable<T> @this, string message, string title = "") =>
        @this
        .Subscribe(
            onNext: val => Debug.Log(title + message + val.ToString()),
            onCompleted: () => Debug.Log($"{title} On completed() "),
            onError: e => Debug.LogError($"{title} onError() {e.Message}"));
}