using UnityEngine;

public class C_PathPoint : I_PathPoint
{
    public C_PathPoint(Vector3 position)
    {
        pathPointPosition = position;
    }
    public Vector3 pathPointPosition;

    public Vector3 PathPointPosition => pathPointPosition;
}
