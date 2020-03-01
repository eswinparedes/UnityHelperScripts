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
    public static IObservable<ASingleSelect<T>> SingleSelectToggler<T>(this IObservable<Option<T>> @this, Func<T, T, bool> equalsComparer) =>
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

    public static IObservable<ASingleSelect<T>> SingleSelectToggler_Published<T>(this IObservable<Option<T>> @this, Func<T, T, bool> equalsComparer)
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