using UnityEngine;

public struct HorizontalMovement
{
    public HorizontalMovement(Vector3 movement, HorizontalMovementType type)
    {
        this.Movement = movement;
        this.Type = type;
    }
    public readonly Vector3 Movement;
    public readonly HorizontalMovementType Type;
}

public enum HorizontalMovementType
{
    None,
    Walk,
    Sprint,
    Dash,
}
