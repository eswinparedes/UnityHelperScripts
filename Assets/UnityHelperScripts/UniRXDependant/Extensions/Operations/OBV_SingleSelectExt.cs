using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static SUHScripts.Functional.Functional;

public static class OBV_SingleSelectExt
{
    /// <summary>
    /// Use Observable.Create ???
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <param name="equalsComparer"></param>
    /// <returns></returns>
    public static IObservable<ASingleSelect<T>> ToggleSelect<T>(this IObservable<Option<T>> @this, Func<T, T, bool> equalsComparer) =>
            @this      
            .Scan(new SingleSelect<T>(), (lastSingleSelect, newIn) =>
            {
                if (lastSingleSelect.Select.IsSome && newIn.IsSome && equalsComparer(lastSingleSelect.Select.Value, newIn.Value))
                {
                    return new SingleSelect<T>(select: NONE, deselect: newIn);
                }
                else
                {
                    return new SingleSelect<T>(select: newIn, deselect: lastSingleSelect.Select);
                }
            });

    public static IObservable<ASingleSelect<T>> ToggleSelect_Published<T>(this IObservable<Option<T>> @this, Func<T, T, bool> equalsComparer)
    {
        var singleSelect = new SingleSelect_Mutable<T>();

        var obvPublish =
            @this
            .Scan(singleSelect, (lastSingleSelect, newIn) =>
            {
                if (lastSingleSelect.Select.IsSome && newIn.IsSome && equalsComparer(lastSingleSelect.Select.Value, newIn.Value))
                {
                    singleSelect.Apply(select: NONE, deselect: newIn);
                }
                else
                {
                    singleSelect.Apply(select: newIn, deselect: lastSingleSelect.Select);
                }

                return singleSelect;
            })
            .Publish();

        obvPublish.Connect();

        return obvPublish;
    }

    public static IObservable<ASingleSelect<T>> NewEntrySelect<T>(this IObservable<Option<T>> source, Func<T, T, bool> comparer) =>
            Observable.Create<ASingleSelect<T>>(observer =>
            {
                Option<T> current = None.Default;
                var selector = new SingleSelect_Mutable<T>();

                return source.Subscribe(
                   onNext: optIn =>
                   {
                       if (current.IsSome)
                       {
                           if (optIn.IsSome) //If our input is some and does not compare to current, select input and deselect current
                           {
                               var doesCompare = comparer(current.Value, optIn.Value);
                               if (!doesCompare)
                               {
                                   selector.Apply(select: optIn.Value, deselect: current.Value);
                                   current = optIn;
                                   observer.OnNext(selector);
                               }
                           }
                           else //If our input is NONE then deselect current
                           {
                               selector.Apply(select: None.Default, deselect: current.Value);
                               current = None.Default;
                               observer.OnNext(selector);
                           }
                       }
                       else
                       {
                           if (optIn.IsSome) //Current is NONE and we have some input value
                           {
                               selector.Apply(select: optIn.Value, deselect: None.Default);
                               current = optIn;
                               observer.OnNext(selector);
                           }
                       }
                   },
                   onCompleted: observer.OnCompleted,
                   onError: observer.OnError);
            });
}



public abstract class ASingleSelect<T>
{
    public abstract Option<T> Select { get; }
    public abstract Option<T> Deselect { get; }
}

class SingleSelect_Mutable<T> : ASingleSelect<T>
{
    public SingleSelect_Mutable()
    {
        m_select = NONE;
        m_deselect = NONE;
    }

    public SingleSelect_Mutable(Option<T> select, Option<T> deselect)
    {
        m_select = select;
        m_deselect = deselect;
    }

    public void Apply(Option<T>? select = null, Option<T>? deselect = null)
    {
        m_select = select ?? m_select;
        m_deselect = deselect ?? m_deselect;
    }
    Option<T> m_select;
    Option<T> m_deselect;

    public override Option<T> Select => m_select;
    public override Option<T> Deselect => m_deselect;
}

class SingleSelect<T> : ASingleSelect<T>
{
    public SingleSelect()
    {
        Select = NONE;
        Deselect = NONE;
    }

    public SingleSelect(Option<T> select, Option<T> deselect)
    {
        Select = select;
        Deselect = deselect;
    }

    public override Option<T> Select { get; }
    public override Option<T> Deselect { get; }
}