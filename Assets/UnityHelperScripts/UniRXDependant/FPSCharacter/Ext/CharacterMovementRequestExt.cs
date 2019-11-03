using UnityEngine;

public static class CharacterMovementRequestExt
{
    public static Vector3 TotalMovement(this MovementRequest @this) =>
        @this.HorizontalMovement.Movement + @this.VerticalMovement.Movement;

    public static bool DidJump(this MovementRequest @this) =>
        @this.VerticalMovement.Type == VerticalMovementType.Jump;

    public static bool IsTerminated(this MovementRequest @this) =>
         @this.RequestStatus == MovementRequestStatus.Ended
        || @this.RequestStatus == MovementRequestStatus.Failed;
}
