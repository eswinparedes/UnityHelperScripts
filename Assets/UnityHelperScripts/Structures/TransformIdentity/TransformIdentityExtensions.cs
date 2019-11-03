using UnityEngine;

public static class TransformIdentityExtensions 
{
    public static Transform ApplyTransformData(Transform target, TransformData data)
    {
        target.position = data.Position;
        target.rotation = data.Rotation;
        target.localScale = data.Scale;
        return target;
    }

    public static Transform ApplyTransformDataOrientation(Transform target, TransformData data)
    {
        target.position = data.Position;
        target.rotation = data.Rotation;
        return target;
    }

    public static Transform ApplyOrientation(this Transform @this, TransformData data) =>
        ApplyTransformDataOrientation(@this, data);

    public static Transform ApplyOrientationToTransform(this TransformData @this, Transform trans) =>
        ApplyTransformDataOrientation(trans, @this);

    public static Transform ApplyIdentity(this Transform @this, TransformData data) =>
        ApplyTransformData(@this, data);

    public static Transform ApplyToTransform(this TransformData @this, Transform target) =>
        ApplyTransformData(target, @this);

    public static TransformData ExtractData(this Transform @this) =>
        new TransformData(@this.position, @this.rotation, @this.localScale);

    public static TransformData ExtractOrientation(this Transform @this) =>
        new TransformData(@this.position, @this.rotation);

    public static TransformData WithPosition(this TransformData @this, Vector3 position) =>
        new TransformData(position, @this.Rotation, @this.Scale);

    public static TransformData WithRotation(this TransformData @this, Quaternion rotation) =>
        new TransformData(@this.Position, rotation, @this.Scale);

    public static TransformData WithScale(this TransformData @this, Vector3 scale) =>
        new TransformData(@this.Position, @this.Rotation, scale);

    public static TransformData With
        (this TransformData @this, Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null) =>
        new TransformData(position?? @this.Position, rotation?? @this.Rotation, scale?? @this.Scale);

    public static float DotProductLookToNormalized(this TransformData lookFrom, TransformData lookTo, Vector3? relativeLHS = null)
    {
        var vector = relativeLHS ?? Vector3.forward;
        Vector3 lhs = lookFrom.Rotation * vector;
        Vector3 rhs = (lookTo.Position - lookFrom.Position).normalized;
        return Vector3.Dot(lhs, rhs);
    }
    public static float DotProductLookTo(this TransformData lookFrom, TransformData lookTo, Vector3? relativeLHS = null)
    {
        var vector = relativeLHS ?? Vector3.forward;
        
        Vector3 lhs = lookFrom.Rotation * vector;
        Vector3 rhs = (lookTo.Position - lookFrom.Position);
        return Vector3.Dot(lhs, rhs);
    }

    /// <summary>
    /// direction transformed from world to local
    /// </summary>
    /// <param name="this"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 TransformDirection(this TransformData @this, Vector3 direction) =>
        @this.Rotation * direction;

    public static Vector3 Forward(this TransformData @this) =>
        @this.TransformDirection(Vector3.forward);

    public static Vector3 Up(this TransformData @this) =>
        @this.TransformDirection(Vector3.up);

    public static Vector3 Right(this TransformData @this) =>
        @this.TransformDirection(Vector3.right);
}
