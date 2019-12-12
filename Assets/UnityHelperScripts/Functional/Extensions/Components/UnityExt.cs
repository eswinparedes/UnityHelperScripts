using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static SUHScripts.Functional.GenericExtensions;
using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;

public static class UnityExt 
{
    public static bool OnComponentFound<T>(this GameObject @this, Action<T> onComponentFound) =>
        @this.HasComponent(out T component) ?
        true.Pass(() => onComponentFound(component)) : false;


    public static bool OnComponentFound<T>(this Component @this, Action<T> onComponentFound) =>
        @this.HasComponent(out T component) ?
        true.Pass(() => onComponentFound(component)) : false;

    public static bool HasComponent<T>(this Component obj, out T component) =>
        (component = obj.GetComponent<T>())
        .MapInto(comp => comp != null);

    public static bool HasComponent<T>(this GameObject @this, out T component) =>
        (component = @this.GetComponent<T>())
        .MapInto(comp => comp != null);

    public static Option<T> GetComponentOption<T>(this GameObject @this) =>
        @this.GetComponent<T>().AsOption();

    public static Option<T> GetComponentOption<T>(this Component @this) =>
        @this.GetComponent<T>().AsOption();

    /// <summary>
    /// Searches for A_QueryComponentSource, if found, queries it.
    /// If no component is found yet, defaults to using GetComponentOption() on the component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static Option<T> QueryComponentOption<T>(this Component @this, bool useGetComponent = true)
    {
        var opt = @this.GetComponentOption<A_QueryComponentSource>();

        var queryOption =
            opt.IsSome
            ? opt.Value.QueryComponentOption<T>()
            : NONE;

        if (queryOption.IsSome)
            return queryOption;
        else
            return useGetComponent ? @this.GetComponentOption<T>() : NONE;
    }

    /// <summary>
    /// Searches for A_QueryComponentSource, if found, queries it.
    /// If no component is found yet, defaults to using GetComponentOption() on the component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static Option<T> QueryComponentOption<T>(this GameObject @this, bool useGetComponent = true)
    {
        var opt = @this.GetComponentOption<A_QueryComponentSource>();

        var queryOption =
            opt.IsSome
            ? opt.Value.QueryComponentOption<T>()
            : NONE;

        if (queryOption.IsSome)
            return queryOption;
        else
            return useGetComponent ? @this.GetComponentOption<T>() : NONE;
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject)
            where T : Component
    {
        var component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}
