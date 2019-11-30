using System.Collections.Generic;
using UnityEngine;
using static ToolSet;

public static class ColliderExtensions
{
    public static (Vector3 bottomLeft, Vector3 bottomRight) GetBottomEdges(this Collider2D @this)
    {
        var ext = @this.bounds.extents;
        var y = @this.bounds.center.y - ext.y;
        var bottomLeft = new Vector3(@this.bounds.center.x - ext.x, y, ext.z);
        var bottomRight = new Vector3(@this.bounds.center.x + ext.x, y, ext.z);
        return (bottomLeft, bottomRight);
    }

    public static IEnumerable<Collider2D> YieldCollidersBelow(this Collider2D @this, LayerMask? layerMask = null, float dist = .1f, int fidelity = 3)
    {
        if (fidelity == 0) yield break;

        var mask = layerMask ?? ~0;
        (var posA, var posB) = @this.GetBottomEdges();

        foreach (var position in YieldPositionsBetween(posA, posB, fidelity))
        {
            var direction = Vector2.down;
            var result = Physics2D.Raycast(position, direction, dist, mask);

            Debug.DrawRay(position, direction * .1f, Color.red);

            if (result.collider != null)
                yield return result.collider;
        }
    }
}