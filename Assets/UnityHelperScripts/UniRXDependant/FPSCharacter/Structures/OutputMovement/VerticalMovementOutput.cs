using UnityEngine;

public struct VerticalMovement
{
    public VerticalMovement(Vector3 movement, VerticalMovementType type)
    {
        this.Movement = movement;
        this.Type = type;
    }
    public readonly Vector3 Movement;
    public readonly VerticalMovementType Type;
}
public enum VerticalMovementType
{
    None,
    Jump,
    Jetpack,
}
