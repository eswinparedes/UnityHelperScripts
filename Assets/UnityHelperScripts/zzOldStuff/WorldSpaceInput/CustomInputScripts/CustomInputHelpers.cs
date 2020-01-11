using UnityEngine;

public class CustomInputHelpers
{
    //Modded function from above
    public static Ray GetRayFromLocal(Vector3 localPosition, Quaternion localOrientation, Transform trackingSpace)
    {
        Matrix4x4 localToWorld = trackingSpace.localToWorldMatrix;
        Vector3 worldStartPoint = localToWorld.MultiplyPoint(localPosition);
        Vector3 worldOrientation = localToWorld.MultiplyVector(localOrientation * Vector3.forward);
        Debug.DrawRay(worldStartPoint, worldOrientation * 100, Color.red);
        return new Ray(worldStartPoint, worldOrientation);
    }

    public static Ray GetRayFromLocal(Transform transformIdentity)
    {
        return new Ray(transformIdentity.position, transformIdentity.forward);
    }
    //Modded function from above
    public static Ray GetRayFromLocal(Vector3 position, Vector3 direction)
    {
        Debug.DrawRay(position, direction * 100, Color.red);
        return new Ray(position, direction);
    }
}