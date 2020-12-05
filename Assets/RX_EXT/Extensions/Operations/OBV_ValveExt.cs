using SUHScripts.Functional;
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

            return new CompositeDisposable(isPlayingSub, obsSub);
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

                return new CompositeDisposable(isPlayingSub, obSub);
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

                return new CompositeDisposable(valveFromSub, obsSub);
            });

        public static IObservable<T> ValveRelease<T>(this IObservable<T> @this, IObservable<bool> valve, Func<T> releaseValueSource, bool startPlaying) =>
            Observable.Create<T>(observer =>
            {
                var isPlaying = startPlaying;
                var releaseSubject = new Subject<T>();

                var sub0 = valve.Subscribe(allow =>
                {
                    if (allow)
                    {
                        isPlaying = true;
                    }
                    else
                    {
                        if (isPlaying)
                        {   
                            releaseSubject.OnNext(releaseValueSource());
                        }

                        isPlaying = false;
                    }
                });

                var sub1 = @this.Where(_ => isPlaying).Merge(releaseSubject).Subscribe(observer);

                return new CompositeDisposable(sub0, sub1);
            });

        public static IObservable<BoolTrifecta> StutterValve(this IObservable<BoolTrifecta> @this, IObservable<bool> valve) =>
        Observable.Create<BoolTrifecta>(observer =>
        {
            BoolTrifecta runningValue = default;

            var isPlaying = true;
            var isAwaiting = false;

            var sub1 = @this.Subscribe(bt =>
            {
                if (isPlaying)
                {
                    if (isAwaiting)
                    {
                        isAwaiting = bt != BoolTrifecta.False;
                    }
                    else
                    {
                        runningValue = bt;
                        observer.OnNext(bt);
                    }
                }
            });

            var sub2 = valve.DistinctUntilChanged().Subscribe(valveOn =>
            {
                isPlaying = valveOn;

                if (valveOn)
                {
                    isAwaiting = runningValue != BoolTrifecta.False;
                }
                else
                {
                    if (runningValue.IsTrue())
                    {
                        observer.OnNext(BoolTrifecta.FalseThisFrame);
                        observer.OnNext(BoolTrifecta.False);
                    }
                    else if (runningValue.IsFalseThisFrame)
                    {
                        observer.OnNext(BoolTrifecta.False);
                    }

                    runningValue = BoolTrifecta.False;
                }

            });

            return new CompositeDisposable(sub1, sub2);
        });
    }

}
