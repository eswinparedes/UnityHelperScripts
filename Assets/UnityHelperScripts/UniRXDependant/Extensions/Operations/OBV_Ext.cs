using System;
using UnityEngine;
using UniRx;

namespace SUHScripts
{
    using Functional;
    using System.Collections.Generic;

    public static class OBV_Ext 
    {
        //SUHS TODO: Take GetComponent and QueryComponent fucntions from the collider observables and
        // put in here then change the colliderobservable operations to simply select (other) and use these operations
        public static IObservable<T> DoLog<T>(this IObservable<T> @this, string message) =>
            @this.Do(value => Debug.Log($"{message} : {value}"));
        public static IObservable<T> SelectSome<T>(this IObservable<Option<T>> @this) =>
            @this
            .Where(opt => opt.IsSome)
            .Select(opt => opt.Value);

        /// <summary>
        /// Returns a stream that only emits when the Option is some and flattens the value out
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="this"></param>
        /// <param name="chooser"></param>
        /// <returns></returns>
        public static IObservable<R> Choose<T, R>(this IObservable<T> @this, Func<T, Option<R>> chooser) =>
            @this
            .Select(chooser)
            .SelectSome();

        /// <summary>
        /// Given a stream of values and a predicate, returns a tuple with the predicate "state" (whether the predicate just failed, has been failing or stopped failing)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IObservable<(BoolTrifecta predicateState, T value)> TrackPredicate<T>(this IObservable<T> stream, Func<T, bool> predicate)
        {
            T tSeed = default;
            (BoolTrifecta codeState, T value) seed = (new BoolTrifecta(), tSeed);

            return stream.Scan(seed, (prev, newVal) => (prev.codeState.GetUpdateFromInput(predicate(newVal)), newVal));
        }

        public static IObservable<int> MergeTriggersIntoIndex<T>(params IObservable<T>[] others)
        {
            IObservable<int>[] selected = new IObservable<int>[others.Length];

            for (int i = 0; i < others.Length; i++)
            {
                var iClosure = i;
                selected[i] = others[i].Select(_ => iClosure);
            }

            return selected.Merge();
        }

        public static IObservable<R> SwitchTo<T, R>(this IObservable<T> @this, Func<T, IObservable<R>> selector) =>
            @this.Select(selector)
            .Switch();

        /// <summary>
        /// Terminates the Observable after the first value fails the predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IObservable<T> TakeWhile_IncludeLast<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            return Observable.Create<T>(observer =>
            {
                return source.Subscribe(
                    onNext: val =>
                    {
                        var shouldContinue = false;
                        try
                        {
                            shouldContinue = predicate(val);
                        }
                        catch (Exception e)
                        {
                            observer.OnError(e);
                            return;
                        }

                        observer.OnNext(val);
                        if (!shouldContinue)
                        {
                            observer.OnCompleted();
                        }
                    },
                    onError: err => observer.OnError(err),
                    onCompleted: () => observer.OnCompleted());
            });
        }

        /// <summary>
        /// Collects the latest value of each Observable from "source" when "selectionStream" emits.  
        /// Then, uses "selector" to select a new value based on the latest collected "source" observable values and the "selectionStream" value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="source"></param>
        /// <param name="selectionStream"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IObservable<R> CombinedLatestScanner<T, U, R>(this IObservable<IObservable<T>> source, IObservable<U> selectionStream, Func<IEnumerable<T>, U, R> selector)
        {
            return Observable.Create<R>(observer =>
            {
                var values = new Dictionary<IObservable<T>, T>();
                var subs = new List<IDisposable>();

                source.Select(_ => Unit.Default)
                .Merge(selectionStream.Select(_ => Unit.Default))
                .Subscribe(onNext: x => { }, onCompleted: () => observer.OnCompleted(), onError: e => observer.OnError(e))
                .AddTo(subs);

                source
                .Subscribe(valueProvider =>
                {
                    var valueProviderClosure = valueProvider;
                    valueProvider
                    .Subscribe(
                        onNext: toScan => values[valueProviderClosure] = toScan,
                        onCompleted: () => values.Remove(valueProviderClosure),
                        onError: e => values.Remove(valueProviderClosure))
                    .AddTo(subs);

                }).AddTo(subs);

                selectionStream
                    .Subscribe(selectionTick => observer.OnNext(selector(values.Values, selectionTick)))
                    .AddTo(subs);

                return subs.AsDisposable();
            });
        }

        /// <summary>
        /// Returns a Selection of the last value of the first observable to complete
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="obv0"></param>
        /// <param name="obv1"></param>
        /// <param name="obv0LastSelector"></param>
        /// <param name="obv1LastSelector"></param>
        /// <returns></returns>
        public static IObservable<R> FirstCompleted<T0, T1, R>(this IObservable<T0> obv0, IObservable<T1> obv1, Func<T0, R> obv0LastSelector, Func<T1, R> obv1LastSelector) =>
            obv0.Last().Select(obv0LastSelector)
            .Merge(obv1.Last().Select(obv1LastSelector))
            .First();

        /// <summary>
        /// Takes "source" observable emissions until "other" or "source" completes or errors
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static IObservable<T0> TakeDuring<T0, T1>(this IObservable<T0> source, IObservable<T1> other) =>
           Observable.Create<T0>(observer =>
           {
               bool terminated = false;
               Action<Exception> onError =
                   e =>
                   {
                       if (terminated) return;
                       observer.OnError(e);
                       terminated = true;
                   };

               Action<T0> onNext =
                   x =>
                   {
                       if (terminated) return;
                       observer.OnNext(x);
                   };

               Action onCompleted =
                   () =>
                   {
                       if (terminated) return;
                       observer.OnCompleted();
                       terminated = true;
                   };

               var sub0 =
                   other.Subscribe(x => { }, onError, onCompleted);

               var sub1 =
                   source.Subscribe(onNext, onError, onCompleted);

               return new CompositeDisposable(sub0, sub1);
           });


        /// <summary>
        /// In Testing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="this"></param>
        /// <param name="result"></param>
        /// <param name="toggleSeed"></param>
        /// <returns></returns>
        public static IObservable<R> ToggleEach<T, R>(this IObservable<T> @this, Func<bool, T, R> result, bool toggleSeed)
        {
            (bool toggleState, T value) seed = (toggleSeed, default);
            return
                @this.Scan(seed, (prev, input) => (!prev.toggleState, input))
                .Select(inputs => result(inputs.toggleState, inputs.value));
        }
        
    }
}
