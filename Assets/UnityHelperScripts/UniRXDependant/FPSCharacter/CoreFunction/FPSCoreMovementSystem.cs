using SUHScripts.Functional;
using UnityEngine;
using MathHelpers;

public static class FPSCoreMovementSystem 
{
    public static Vector3 RelativeTo(this Vector3 @this, CharacterControllerState state) =>
             state.TransformDirection(@this);

    public static Vector3 Multiply(this Vector3 @this, float value) =>
        @this * value;

    public static Vector3 RelativeMovement(this Vector2 @this, CharacterControllerState relativeForward, float speed) =>
        @this
        .AsXZ()
        .normalized
        .RelativeTo(relativeForward)
        .AlongNormal(relativeForward.ReducedSurfaceNormal())
        .Multiply(speed);


    static float GetSpeed(this StandardMovement @this, bool useBoost) =>
            useBoost ? @this.BoostSpeed : @this.BaseSpeed;

    static Vector3 OptionallyAccelerateMagnitudeFrom
        (this Vector3 desired, Vector3 from, StandardMovement settings, float delta) =>
        settings
        .Acceleration
        .Match(
            () => desired,
            acc => from.AccelerateMagnitudeTowardsDesiredVector(desired, acc * delta));

    public static Vector3 AcceleratedMagnitudeMovement
        (this StandardMovement @this, CharacterControllerState state, Vector2 input, bool useBoost, float delta) =>
        input
        .RelativeMovement(state, @this.GetSpeed(useBoost))
        .OptionallyAccelerateMagnitudeFrom(state.HorizontalVelocity(), @this, delta);

    public static Vector3 StandardGravityOrStick(CharacterControllerState state, GravityData gravityData, float maxUpwardVelocity, float deltaTime)
    {
        var vspd = 
            state.IsGrounded.IsTrue
            ? gravityData.Gravity 
            : Mathf.Clamp(gravityData.Gravity * deltaTime + state.TransformDirection(state.Velocity).y, gravityData.TerminalVelocity, maxUpwardVelocity);

        return vspd * state.TransformDirection(Vector3.up);
    }

    //Rotates camera along x and y additively
    public static Quaternion GetRotationFromInputAxis(Vector2 input, FPSCameraSettings settings, Quaternion localRotation)
    {
        var inputLook = input * settings.Sensitivity;
        var horzLook = inputLook.x * Time.deltaTime * Vector2.up;
        var rotationHorizontal = localRotation * Quaternion.Euler(horzLook);

        var vertLook = inputLook.y * Time.deltaTime * Vector2.left;
        var newQ = rotationHorizontal * Quaternion.Euler(vertLook);

        var euler =
            MathHelper.ClampRotationAroundXAxis(newQ, -settings.MaxViewAngle, -settings.MinViewAngle)
            .eulerAngles
            .WithZ(0);

        return Quaternion.Euler(euler);
    }
}