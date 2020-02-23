using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SUHScripts
{
    public static class OBV_ValveExt     
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

        public static IObservable<bool> ValveFrom<U, V>(IObservable<U> onStream, IObservable<V> offStream, bool startPlaying) =>
         Observable.Create<bool>(observer =>
         {
             var isPlaying = startPlaying;

             var sub =
             onStream.Select(_ => true)
             .Merge(offStream.Select(_ => false))
             .Subscribe(observer.OnNext, observer.OnError, observer.OnCompleted);

             return sub;
         });

        public static IObservable<T> ValveBy<T, U, V>(this IObservable<T> @this, IObservable<U> onStream, IObservable<V> offStream, bool startPlaying) =>
            Observable.Create<T>(observer =>
            {
                var isPlaying = startPlaying;

                var valveFromSub =
                    ValveFrom(onStream, offStream, startPlaying)
                    .Subscribe(setStartPlaying => isPlaying = setStartPlaying);

                var obsSub =
                    @this.Where(_ => isPlaying)
                    .Subscribe(observer.OnNext, observer.OnError, observer.OnCompleted);

                return SUHScripts.DisposableExtensions.AsDisposable(valveFromSub, obsSub);
            });

    }

}
