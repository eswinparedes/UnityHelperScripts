using System;
using UniRx;
using UnityEngine;

public class CHAR_DashInstance : CHAR_MovementInstance
{
    public override CharacterMovementFunction ProcessMovement { get; protected set; }
    public override Action Enter { get; protected set; }
    public override Action Exit { get; protected set; }
    public override CanEnterValidation CanEnter { get; protected set; }

    public override void PushState(CharacterMovementOutput state) =>
        MUTABLE_STATE_CACHE = state;
    public override CharacterMovementOutput GetState() =>
        MUTABLE_STATE_CACHE;

    private readonly Func<DashConstant> dashSource;
    private readonly Func<float> alphaSource;

    private CharacterMovementOutput MUTABLE_STATE_CACHE;
    private FTimer MUTABLE_DASH_TIMER;
    private FTimer MUTABLE_DASH_COOLDOWN_TIMER;

    public CHAR_DashInstance(FPSRoot root, CHAR_Dash data)
    {
        //Seed values
        MUTABLE_STATE_CACHE = root.Character.BuildSeed();
        MUTABLE_DASH_TIMER = new FTimer(data.DashSettings.DashTimeLength, 0, true);
        MUTABLE_DASH_COOLDOWN_TIMER = new FTimer(data.DashSettings.CoolDownLength, data.DashSettings.CoolDownLength, true);

        //Insert Dependencies
        dashSource = () => data.DashSettings;
        alphaSource = () => MUTABLE_DASH_TIMER.TimeAlpha();

        var dashToView = GetDashToViewFunction(root);
        var dashToInput = GetDashToInputFunction();

        CanEnter = () => MUTABLE_DASH_COOLDOWN_TIMER.HasCompleted();

        Enter =
            () =>
            {
                //STATE MUTATION ----------
                MUTABLE_DASH_TIMER = MUTABLE_DASH_TIMER.Restarted();
                MUTABLE_DASH_COOLDOWN_TIMER = MUTABLE_DASH_COOLDOWN_TIMER.Restarted();
                //------------------
            };

        Exit = () => { };

        ProcessMovement =
            inputs =>
            {
                var inputState =
                    MUTABLE_STATE_CACHE
                    .WithUpdatedCharacterController(root.Character, false);

                var activeFunction = data.DoesDashToView ? dashToView : dashToInput;
                var movementRequest = activeFunction(inputs, MUTABLE_STATE_CACHE);

                //STATE MUTATION ---------------
                MUTABLE_STATE_CACHE = 
                    root.Character.MoveAndOutput(inputState, movementRequest);

                MUTABLE_DASH_TIMER = 
                    MUTABLE_DASH_TIMER.Tick(inputs.deltaTime);
                //--------------------

                return MUTABLE_STATE_CACHE;
            };

        root
            .OnUpdate
            .Subscribe(deltaTime => MUTABLE_DASH_COOLDOWN_TIMER = MUTABLE_DASH_COOLDOWN_TIMER.Tick(deltaTime))
            .AddTo(root.AttachBehaviour);
    }


    private Func<MoveInputs, CharacterMovementOutput, MovementRequest> GetDashToViewFunction(FPSRoot root)
    {
        var dash = MovementClassFunctions.DashToVectorFunction(dashSource, alphaSource);
        var MUTABLE_DASH_VECTOR = Vector3.zero;

        return
            (inputs, state) =>
            {
                MUTABLE_DASH_VECTOR =
                    alphaSource() != 0
                    ? MUTABLE_DASH_VECTOR
                    : root.FPSCamera.RootTransform.forward;

                return dash((MUTABLE_DASH_VECTOR, inputs.deltaTime));
            };
    }

    private Func<MoveInputs, CharacterMovementOutput, MovementRequest> GetDashToInputFunction()
    {
        var dash = MovementClassFunctions.DashFunction(dashSource, alphaSource);
        var MUTABLE_DASH_INPUT = Vector2.zero;

        return
            (inputs, state) =>
            {
                //STATE MUTATION
                MUTABLE_DASH_INPUT =
                alphaSource() != 0
                ? MUTABLE_DASH_INPUT
                : inputs.localMovement == Vector2.zero
                ? Vector2.up
                : inputs.localMovement;
                //-------------------

                return dash((MUTABLE_DASH_INPUT, inputs.deltaTime), state.OutputControllerState);
            };
    }
}
