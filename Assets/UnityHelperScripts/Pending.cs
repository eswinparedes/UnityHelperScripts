using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SUHScripts
{
    public static class Pending
    {
        public static IObservable<T> Valve<T>(this IObservable<T> @this, IObservable<bool> playSteam, bool startPlaying) =>
        Observable.Create<T>(observer =>
        {
            var isPlaying = startPlaying;

            var isPlayingSub = playSteam.Subscribe(isPlayingInput => isPlaying = isPlayingInput);

            var obsSub =
            @this
            .Where(_ => isPlaying)
            .Subscribe(observer.OnNext, observer.OnError, observer.OnCompleted);

            return SUHScripts.DisposableExtensions.AsDisposable(isPlayingSub, obsSub);
        });

        public static IObservable<R> ValveInjection<T, R>(
            this IObservable<T> @this, IObservable<bool> playStream, bool startPlaying, Func<T, R> valveOn, Func<T, R> valveOff) =>
            Observable.Create<R>(observer =>
            {
                var isPlaying = startPlaying;
                var isPlayingSub = playStream.Subscribe(isPlayingInput => isPlaying = isPlayingInput);

                var obSub =
                @this
                .Select(value => isPlaying ? valveOn(value) : valveOff(value))
                .Subscribe(observer.OnNext, observer.OnError, observer.OnCompleted);

                return SUHScripts.DisposableExtensions.AsDisposable(isPlayingSub, obSub);
            });

        public static void ModifyToRouteComponentQueriesTo(GameObject routTo, params Collider[] colliders)
        {
            if (colliders.Length == 0)
            {
                Debug.LogError("'colliders' array has no values, was this on purpose?");
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                var source = colliders[i].gameObject.GetOrAddComponent<PushableQuerySource>();
                source.PushSource(routTo);
            }
        }

        class PushableQuerySource : A_QueryComponentSource
        {
            GameObject m_source;

            public void PushSource(GameObject source)
            {
                m_source = source;
            }
            public override Option<T> QueryComponentOption<T>() =>
                m_source.GetComponentOption<T>();
        }

        public static void AddedReturn<T>(this Subject<T> subject, Component addTo, T value)
        {
            subject.AddTo(addTo);
            subject.OnNext(value);
            subject.OnCompleted();
        }

        public static void AddedReturn<T>(this ReplaySubject<T> subject, Component addTo, T value)
        {
            subject.AddTo(addTo);
            subject.OnNext(value);
            subject.OnCompleted();
        }
    }
}

