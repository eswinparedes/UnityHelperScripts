using SUHScripts.Functional;
using UnityEngine;

public static class CharacterMovemntOutputExt
{
    public static bool IsGrounded(this CharacterMovementOutput @this) =>
        @this.OutputControllerState.IsGrounded();

    public static bool DidJump(this CharacterMovementOutput @this) =>
        @this.OutputMovementRequest.VerticalMovement.Type == VerticalMovementType.Jump;

    public static bool DidDash(this CharacterMovementOutput @this) =>
        @this.OutputMovementRequest.HorizontalMovement.Type == HorizontalMovementType.Dash;

    static bool OptionEquals(this Option<int> @this, int jumpsLeft) =>
        @this.ReducedGet(-jumpsLeft) == jumpsLeft;

    static bool ShouldDecrementJumps(this CharacterMovementOutput @this, 
        Option<int> maxJumps, int currentJumpsLeft) =>
        @this.DidJump() || 
        (maxJumps.OptionEquals(currentJumpsLeft) && !@this.IsGrounded());

    static int DecrementOption(this CharacterMovementOutput @this,
        Option<int> maxBoosts, int current) =>
        @this.ShouldDecrementJumps(maxBoosts, current)
        ? current - 1 : current;

    public static int JumpConsume(this CharacterMovementOutput @this,
        Option<int> maxBoosts, int currentJumps) =>
        @this.IsGrounded()
        ? maxBoosts.Reduce(1)
        : @this.ShouldDecrementJumps(maxBoosts, currentJumps)
        ? @this.DecrementOption(maxBoosts, currentJumps)
        : currentJumps;

    public static CharacterMovementOutput WithUpdatedCharacterController(
        this CharacterMovementOutput @this, CharacterController character, bool updateGrounding)
    {
        return
            new CharacterMovementOutput(
                character.ExtractState(@this.OutputControllerState, updateGrounding),
                @this.OutputMovementRequest);
    }

    public static bool IsWalkRun(this CharacterMovementOutput @this) =>
        @this.OutputMovementRequest.HorizontalMovement.Type == HorizontalMovementType.Walk
        || @this.OutputMovementRequest.HorizontalMovement.Type == HorizontalMovementType.Sprint;
}