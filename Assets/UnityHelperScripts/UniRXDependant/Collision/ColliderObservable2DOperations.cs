using SUHScripts.Functional;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public static class ColliderObservable2DOperations
{
    public static IObservable<(Option<Collision2D> col, Collider2D other)> OnEnterAny(
        this IEnumerable<A_ColliderObservable2D> @this) =>
        @this
        .Select(obs => obs.OnEnter)
        .Merge();

    public static IObservable<(Option<Collision2D> col, Collider2D other)> OnExitAny(
        this IEnumerable<A_ColliderObservable2D> @this) =>
        @this
        .Select(obs => obs.OnExit)
        .Merge();

    //SUHS TODO: Make these dictionary functions into generic operations
    public static bool Any(this Dictionary<Collider2D, int> @this, Collider2D col)
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

    //SUHS TODO: This should be in ColliderObservable2DBehaviours not Operations since this is sideffecting
    public static void Increment(this Dictionary<Collider2D, int> @this, Collider2D col)
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

    public static void Decrement(this Dictionary<Collider2D, int> @this, Collider2D col)
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

    //SUHS TODO: Redundante use of getcomponent and then query compeonent?
    //SUHS TODO: Can chain with onenter componentoption?
    //SUHS TODO: Do the same for exit
    //SUHS TODO: Track the lifetime of (enter/exit) of an object
    public static IObservable<Option<T>> OnEnterQueryComponent<T>(this A_ColliderObservable2D @this) =>
        @this
        .OnEnter
        .Select(entered => entered.other.GetComponentOption<A_QueryComponentSource>())
        .Where(item => item.IsSome)
        .Select(item => item.Value.QueryComponentOption<T>());

    public static IObservable<Option<T>> OnEnterComponentOption<T>(this A_ColliderObservable2D @this) =>
        @this
        .OnEnter
        .Select(entered => entered.other.GetComponentOption<T>());

    public static IObservable<T> OnEnterQuerySome<T>(this A_ColliderObservable2D @this) =>
        @this
        .OnEnterQueryComponent<T>()
        .SelectSome();

    public static IObservable<T> OnEnterGetComponentSome<T>(this A_ColliderObservable2D @this) =>
        @this
        .OnEnterComponentOption<T>()
        .SelectSome();
}
