using UnityEngine;
using System;
using static FPSCoreMovementSystem;

public static class FPSCoreMovementFunctions 
{
    //Movement output Functions
    public static Func<(bool attemptJump, float deltaTime, CharacterControllerState inputState), VerticalMovement>
        JumpFallStickFunction(
        Func<DashInstant> jumpSettings,
        Func<GravityData> gravityDataSource)
    {
        return
            (inputs) =>
            {
                //Simplify source values
                var gravityData = gravityDataSource();
                var _jumpSettings = jumpSettings();
                //ProcessJumping
                var requestedVerticalVector =
                    inputs.attemptJump ?
                        inputs.inputState.TransformDirection(Vector3.up * _jumpSettings.BoostSpeed) :
                        StandardGravityOrStick(inputs.inputState, gravityData, _jumpSettings.BoostSpeed, inputs.deltaTime);

                var VertType = inputs.attemptJump ? VerticalMovementType.Jump : VerticalMovementType.None;
                return new VerticalMovement(requestedVerticalVector, VertType);
            };
    }

    public static Func<(bool trySprint, Vector2 localMovement, float deltaTime), CharacterControllerState, HorizontalMovement>
        WalkRunFunction(Func<StandardMovement> walkRunSource)
    {
        return
        (inputs, lastState) =>
        {
            var trySprint = 
                inputs.trySprint && inputs.localMovement.ValueInForwardDot(.7f); //Make sure sprint input is within cone

            var type =
                trySprint ? HorizontalMovementType.Sprint : HorizontalMovementType.Walk;

            var requestedHorizontalVector =
                walkRunSource()
                .AcceleratedMagnitudeMovement(lastState, inputs.localMovement, trySprint, inputs.deltaTime);

            return new HorizontalMovement(requestedHorizontalVector, type);
        };
    }

    public static Func<Vector2, CharacterControllerState, HorizontalMovement>
        HorizontalDashingFunction(Func<DashConstant> dashStatsSource, Func<float> alphaSource)
    {
        return
            (input, lastState) =>
            {
                var dashStats = dashStatsSource();

                var requestedVector =
                    lastState.TransformDirection(input.AsXZ() * dashStats.BoostSpeed);

                return new HorizontalMovement(requestedVector * dashStats.DashCurve.Evaluate(alphaSource()), HorizontalMovementType.Dash);
            };
    }
}
