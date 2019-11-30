using SUHScripts.Functional;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public static class ColliderObservableOperations 
{
    public static IObservable<(Option<Collision> col, Collider other)> OnEnterAny(
        this IEnumerable<A_ColliderObservable> @this) =>
        @this
        .Select(obs => obs.OnEnter)
        .Merge();

    public static IObservable<(Option<Collision> col, Collider other)> OnExitAny(
        this IEnumerable<A_ColliderObservable> @this) =>
        @this
        .Select(obs => obs.OnExit)
        .Merge();

    public static bool Any(this Dictionary<Collider, int> @this, Collider col)
    {
        if (@this.ContainsKey(col))
        {
            return @this[col] > 0;
        }
        else
        {
            return false;
        }
    }
        

    public static void Increment(this Dictionary<Collider, int> @this, Collider col)
    {
        if (@this.ContainsKey(col))
        {
            @this[col] = @this[col] + 1;
        }
        else
        {
            @this.Add(col, 1);
        }
    }

    public static void Decrement(this Dictionary<Collider, int> @this, Collider col)
    {
        if (@this.ContainsKey(col))
        {
            @this[col] = @this[col] - 1;

            if (@this[col] <= 0)
            {
                @this.Remove(col);
            }
        }
    }

    public static IObservable<Option<T>> OnEnterComponentOption<T>(this A_ColliderObservable @this) =>
        @this
        .OnEnter
        .Select(entered => entered.other.GetComponentOption<T>());

    //SUHS TODO: Do the same for exit
    //SUHS TODO: Track the lifetime of (enter/exit) of an object
    public static IObservable<Option<T>> OnEnterQueryComponent<T>(this A_ColliderObservable @this) =>
        @this
        .OnEnter
        .Select(item => item.other.QueryComponentOption<T>());

    public static IObservable<T> OnEnterQuerySome<T>(this A_ColliderObservable @this) =>
        @this
        .OnEnterQueryComponent<T>()
        .SelectSome();

    public static IObservable<T> OnEnterGetComponentSome<T>(this A_ColliderObservable @this) =>
        @this
        .OnEnterComponentOption<T>()
        .SelectSome();
}
