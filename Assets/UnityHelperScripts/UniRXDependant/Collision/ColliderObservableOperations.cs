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
}
