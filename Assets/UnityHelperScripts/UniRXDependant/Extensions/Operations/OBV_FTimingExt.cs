using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SUHScripts
{
    public static class OBV_FTimingExt
    {
        public static IObservable<(FTimer timer, T value)> TrackTimer<T>(this IObservable<T> @this, float time, Func<FTimer, T, FTimer> timerUpdater)
        {
            T tSeed = default;
            var timerSeed = new FTimer(time, 0);
            var initSeed = new FTimer(time, time);
            (FTimer timer, T value) seed = (initSeed, tSeed);

            return
                @this.Scan(
                    seed,
                    (prev, value) =>
                    {
                        var nextTimer =
                            prev.timer.HasCompleted()
                            ? timerSeed
                            : timerUpdater(prev.timer, value);

                        return (nextTimer, value);
                    });
        }

        public static IObservable<R> TrackTimer<T, R>(this IObservable<T> @this, float time, Func<FTimer, T, FTimer> timerUpdater, Func<FTimer, T, R> selector) =>
            @this.TrackTimer(time, timerUpdater)
            .Select(inputs => selector(inputs.timer, inputs.value));

        public static IObservable<(T value, FTimer timer)> TimerScan<T>(this IObservable<T> @this, Func<T, float> tickFunction, Func<FTimer> seed)
        {
            (T value, FTimer timer) seedInput = (default, seed());

            return
            @this.Scan(
                    seedInput,
                    (state, value) => (value, state.timer.Tick(tickFunction(value))))
                .TakeWhile_IncludeLast(inputs => !inputs.timer.HasCompleted());
        }
    }

}
