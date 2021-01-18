using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SUHScripts;
using SUHScripts.Functional;

public static class UPDATS
{
    public static bool StateIsAtOrPastNormalTime(this Animator @this, string state, float time, int layerIndex = 0)
    {
        var info = @this.GetCurrentAnimatorStateInfo(layerIndex);
        return info.IsName(state) && info.normalizedTime >= time;
    }
    public static Option<T> TryClosest<T>(this IReadOnlyList<T> @this, Vector2 point) where T : Component
    {
        var count = @this.Count;

        if (count == 0) return None.Default;

        Option<T> nearest = None.Default;
        var nearestDist = float.MaxValue;

        foreach (var i in @this)
        {
            var dist = Vector2.Distance(i.transform.position, point);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = i.AsOption_SAFE();
            }
        }

        return nearest;

    }

    public static Option<T> TryClosest<T>(this IReadOnlyList<T> @this, Vector3 point) where T : Component
    {
        var count = @this.Count;

        if (count == 0) return None.Default;

        Option<T> nearest = None.Default;
        var nearestDist = float.MaxValue;

        foreach (var i in @this)
        {
            var dist = Vector3.Distance(i.transform.position, point);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = i.AsOption_SAFE();
            }
        }

        return nearest;

    }

    public static Option<TGetComponent> TryClosestComponentWhere<TComponent, TGetComponent>(this IReadOnlyList<TComponent> @this, Func<TGetComponent, bool> where, Vector3 point) where TComponent : Component
    {
        var count = @this.Count;

        if (count == 0) return None.Default;

        Option<TGetComponent> nearest = None.Default;
        var nearestDist = float.MaxValue;

        for (int i = 0; i < @this.Count; i++)
        {
            var component = @this[i];
            var getComponentOption = component.GetComponentOption<TGetComponent>();
            var dist = Vector3.Distance(component.transform.position, point);

            if (!getComponentOption.IsSome) continue;

            var isValid = where(getComponentOption.Value);

            if (!isValid) continue;

            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = getComponentOption;
            }
        }

        return nearest;
    }

    public static float Remap(float inValue, float inMin, float inMax, float outMin, float outMax) =>
            outMin + (inValue - inMin) * (outMax - outMin) /
            (inMax - inMin);
    public static void AddTransitionsFor(this StateMachine @this, Func<bool> predicate, IState toState, params IState[] fromStates)
    {
        for (int i = 0; i < fromStates.Length; i++)
        {
            @this.AddTransition(fromStates[i], toState, predicate);
        }
    }
    public static Quaternion XLookRotation(Vector3 right, Vector3 up = default)
    {
        if (up == default)
            up = Vector3.up;

        Quaternion rightToForward = Quaternion.Euler(0f, -90f, 0f);
        Quaternion forwardToTarget = Quaternion.LookRotation(right, up);

        return forwardToTarget * rightToForward;
    }

    public static Quaternion XLookRotation2D(Vector3 right)
    {
        Quaternion rightToUp = Quaternion.Euler(0f, 0f, 90f);
        Quaternion upToTarget = Quaternion.LookRotation(Vector3.forward, right);

        return upToTarget * rightToUp;
    }

    public static uint ApplyDeltaClamped(this uint @this, int delta)
    {
        if (delta < 0)
        {
            if (Mathf.Abs(delta) > @this)
            {
                return 0u;
            }
            else
            {
                return @this - (uint)delta;
            }
        }
        else
        {
            return @this + (uint)delta;
        }

    }
}
