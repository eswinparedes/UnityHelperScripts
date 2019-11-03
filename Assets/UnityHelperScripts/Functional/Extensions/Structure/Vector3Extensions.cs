using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class Vector3Extensions
{
    public static (float x, float y, float z) Deconstruct(this Vector3 @this) =>
        (@this.x, @this.y, @this.z);

    public static Vector3 Vector3Construct(this (float, float, float) @this) =>
        new Vector3(@this.Item1, @this.Item2, @this.Item3);

    public static Vector3 WithX(this Vector3 @this, float x) =>
        new Vector3(x, @this.y, @this.z);

    public static Vector3 WithY(this Vector3 @this, float y) =>
        new Vector3(@this.x, y, @this.z);

    public static Vector3 WithZ(this Vector3 @this, float z) =>
        new Vector3(@this.x, @this.y, z);

    public static Vector3 WithX(this Vector3 @this, Func<float, float> xMap) =>
        new Vector3(xMap(@this.x), @this.y, @this.z);

    public static Vector3 WithY(this Vector3 @this, Func<float, float> yMap) =>
        new Vector3(@this.x, yMap(@this.y), @this.z);

    public static Vector3 WithZ(this Vector3 @this, Func<float, float> zMap) =>
        new Vector3(@this.x, @this.y, zMap(@this.z));

    public static Vector3 With(this Vector3 @this, float? x = null, float? y = null, float? z = null) =>
        (x == null ? @this.x : x.Value, y == null ? @this.y : y.Value, z == null ? @this.z : z.Value)
        .Vector3Construct();

    public static Vector3 Average(this IEnumerable<Vector3> @this) =>
        @this.Aggregate((x, y) => x + y) / @this.Count();

    public static Vector3 AccelerateTowardsDesiredVectorClamped(this Vector3 current, Vector3 desired, float acceleration)
    {
        var dir = desired - current;
        var mag = dir.magnitude;
        var accel = mag > acceleration ? acceleration : mag;
        return current + dir.normalized * accel;
    }

    public static Vector3 AccelerateMagnitudeTowardsDesiredVector
        (this Vector3 from, Vector3 to, float acceleration)
    {
        var magTarget = to.magnitude;
        var magCurrent = from.magnitude;
        var targetNormalized = to.normalized;

        if (to != Vector3.zero && magCurrent == 0)
        {
            return targetNormalized * acceleration;
        }

        var mag = Mathf.MoveTowards(magCurrent, magTarget, acceleration);
        return targetNormalized * mag;
    }

    public static Vector3 AlongNormal(this Vector3 @this, Vector3 normal) =>
        Vector3.ProjectOnPlane(@this, normal).normalized;

}