using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToolSet
{
    public static IEnumerable<Vector2> YieldPositionsBetween(Vector3 a, Vector3 b, int fidelity)
    {
        var processed = fidelity > 1 ? fidelity : 1;
        var dir = (b - a).normalized;
        var dist = Vector3.Distance(a, b);
        var segment = dist / processed;

        if (processed == 1)
        {
            yield return a + (dir * (dist / 2));
        }
        else
        {
            for (int i = 0; i < processed; i++)
            {
                yield return a + (dir * (i * segment));
            }
        }
    }

    public static T Choose<T>(params T[] items) =>
        items.RandomElement();
}
