using SUHScripts.Functional;
using System;
using UnityEngine;
using UniRx;

public static class ObservableExtensions 
{
    //SUHS TODO: Take GetComponent and QueryComponent fucntions from the collider observables and
    // put in here then change the colliderobservable operations to simply select (other) and use these operations
    public static IObservable<T> DoLog<T>(this IObservable<T> @this, string message) =>
        @this.Do(value => Debug.Log($"{message} : {value}"));
    public static IObservable<T> SelectSome<T>(this IObservable<Option<T>> @this) =>
        @this
        .Where(opt => opt.IsSome)
        .Select(opt => opt.Value);

    public static IObservable<R> Choose<T, R>(this IObservable<T> @this, Func<T, Option<R>> chooser) =>
        @this
        .Select(chooser)
        .SelectSome();

    public static IObservable<(BoolTrifecta predicateState, T value)> TrackPredicate<T>(this IObservable<T> stream, Func<T, bool> predicate)
    {
        T tSeed = default;
        (BoolTrifecta codeState, T value) seed = (new BoolTrifecta(), tSeed);

        return stream.Scan(seed, (prev, newVal) => (prev.codeState.GetUpdateFromInput(predicate(newVal)), newVal));
    }

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
    /// <summary>
    /// NOT TESTED PROPERLY from
    /// https://stackoverflow.com/questions/14697658/rx-observable-takewhile-checks-condition-before-each-element-but-i-need-to-perfo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IObservable<T> TakeWhileInclusive<T>(
    this IObservable<T> source, Func<T, bool> predicate)
    {
        return source.TakeWhile(predicate)
            .Merge(source.SkipWhile(predicate).Take(1));
    }

}
