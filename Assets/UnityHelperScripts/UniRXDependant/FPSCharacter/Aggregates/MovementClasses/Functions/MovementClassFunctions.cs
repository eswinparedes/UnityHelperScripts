using System;
using UnityEngine;

using static FPSCoreMovementFunctions;
public static class MovementClassFunctions
{
    public static Func<MoveInputs, CharacterControllerState, MovementRequest>
        WalkRunJumpFunction(
        Func<StandardMovement> walkRunSettingsSource,
        Func<DashInstant> jumpSettingsSource,
        Func<GravityData> gravityDataSource,
        Func<int> jumpsLeftSource)
    {
        var walkRunFunction = WalkRunFunction(walkRunSettingsSource);
        var jumpFallStickFunction = JumpFallStickFunction(jumpSettingsSource, gravityDataSource);

        return
            (inputs, lastState) =>
            {
                var shouldJump = inputs.jumpState.IsTrueThisFrame && jumpsLeftSource() > 0;
                var requestedHorizontal = walkRunFunction((inputs.sprint.IsTrue, inputs.localMovement, inputs.deltaTime), lastState);
                var requestedVertical = jumpFallStickFunction((shouldJump, inputs.deltaTime, lastState));
                var status = MovementRequestStatus.Executing;
                return new MovementRequest(requestedHorizontal, requestedVertical, status, inputs.deltaTime);
            };
    }

    static MovementRequestStatus FromAlphaStatus(this float alpha) =>
        alpha == 0
            ? MovementRequestStatus.PendingExecution
            : alpha >= 1
            ? MovementRequestStatus.Ended
            : MovementRequestStatus.Executing;

    public static Func<(Vector2 localMovement, float deltaTime), CharacterControllerState, MovementRequest>
        DashFunction(Func<DashConstant> dashSettingsSource, Func<float> alphaSource)
    {
        var dash = HorizontalDashingFunction(dashSettingsSource, alphaSource);

        return
        (inputs, lastState) =>
        {
            var dashStats = dashSettingsSource();

            var requestedVector =
               inputs.localMovement.RelativeMovement(lastState, dashStats.BoostSpeed);

            var horizontalRequest = dash(inputs.localMovement, lastState);
            var verticalRequest = new VerticalMovement(Vector3.zero, VerticalMovementType.None);

            var status = 
                alphaSource()
                .FromAlphaStatus();

            return new MovementRequest(horizontalRequest, verticalRequest, status, inputs.deltaTime);
        };
    }

    public static Func<(Vector3 moveVector, float deltaTime), MovementRequest> DashToVectorFunction(
        Func<DashConstant> dashSettingsSource, Func<float> alphaSource)
    {
        return
            input  =>
            {
                var dashStats = dashSettingsSource();

                var requestedVector =
                    input.moveVector * dashStats.BoostSpeed;

                var movement = requestedVector * dashStats.DashCurve.Evaluate(alphaSource());

                var horizontal = new HorizontalMovement(movement, HorizontalMovementType.Dash);
                var vertical = new VerticalMovement(Vector3.zero, VerticalMovementType.None);

                var status =
                    alphaSource()
                    .FromAlphaStatus();

                return new MovementRequest(horizontal, vertical, status, input.deltaTime);
            };
    }
}
