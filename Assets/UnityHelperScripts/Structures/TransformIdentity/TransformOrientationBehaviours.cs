using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformOrientationBehaviours 
{
    public static Transform ApplyTransformOrientation(Transform target, TransformOrientation data)
    {
        target.position = data.Position;
        target.rotation = data.Rotation;
        return target;
    }

    public static Transform ApplyOrientation(this Transform @this, TransformOrientation data) =>
        ApplyTransformOrientation(@this, data);

    public static Transform ApplyToTransform(this TransformOrientation @this, Transform target) =>
        ApplyTransformOrientation(target, @this);

    public static void ApplyOrientation(this Transform @this, Transform other) =>
        other.ExtractOrientation()
        .ApplyToTransform(other);

    //SUHS TODO: Apply Orientation with offset for rotation and position

    //SUHS TODO: Simulate parent / childe
}
