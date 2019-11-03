using System;

public class CHAR_WRJInstance : CHAR_MovementInstance
{
    public override CharacterMovementFunction ProcessMovement { get; protected set; }
    public override Action Enter { get; protected set; }
    public override Action Exit { get; protected set; }
    public override CanEnterValidation CanEnter { get; protected set; }
    public override void PushState(CharacterMovementOutput state) =>
        MUTABLE_STATE_CACHE = state;
    public override CharacterMovementOutput GetState() =>
        MUTABLE_STATE_CACHE;

    CharacterMovementOutput MUTABLE_STATE_CACHE;

    public CHAR_WRJInstance(FPSRoot root, CHAR_WalkRunJump data)
    {
        MUTABLE_STATE_CACHE = root.Character.BuildSeed();
        var MUTABLE_JUMPS_LEFT = 0;

        var movement =
            MovementClassFunctions.
            WalkRunJumpFunction(
                () => data.MovementSettings,
                () => data.JumpSettings,
                () => root.GravityData,
                () => MUTABLE_JUMPS_LEFT);

        CanEnter = () => true;
        Enter = () => { };
        Exit = () => { };

        ProcessMovement =
            inputs =>
            {
                var inputState =
                    MUTABLE_STATE_CACHE
                    .WithUpdatedCharacterController(root.Character, false);

                var movementRequest =
                    movement(inputs, inputState.OutputControllerState);

                //STATE MUTATION
                MUTABLE_STATE_CACHE =
                    root.Character.MoveAndOutput(inputState, movementRequest);

                MUTABLE_JUMPS_LEFT =
                    MUTABLE_STATE_CACHE
                    .JumpConsume(data.JumpSettings.MaxBoosts, MUTABLE_JUMPS_LEFT);
                //-------------------

                return MUTABLE_STATE_CACHE;
            };
    }
}