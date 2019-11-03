using UnityEngine;
using SUHScripts.Functional;
using System;

public static class CharacterControllerStateExt
{
    public static CharacterControllerState ExtractState(this CharacterController @this, CharacterControllerState lastState, bool updateGrounding) =>
        new CharacterControllerState(
            @this.velocity,
            @this.transform.ExtractData(),
            @this.CharacterGroundingCast().HitData,
            updateGrounding ? lastState.IsGrounded.GetUpdateFromInput(@this.isGrounded) : lastState.IsGrounded);

    /// <summary>
    /// Direction Transformed from world to local
    /// </summary>
    /// <param name="this"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 TransformDirection(this CharacterControllerState @this, Vector3 direction) =>
        @this
        .TransformData
        .TransformDirection(direction);

    public static float GetHorizontalMagnitude(this CharacterControllerState @this) =>
        @this.HorizontalVelocity().magnitude;

    public static bool IsDecelleratingHorizontal(this CharacterControllerState @This, float targetSpeed) =>
        This.GetHorizontalMagnitude() > targetSpeed;

    public static Vector3 HorizontalVelocity(this CharacterControllerState @this) =>
        new Vector3(@this.Velocity.x, 0, @this.Velocity.z);

    public static CharacterMovementOutput BuildSeed
        (this CharacterController @this) =>
            new CharacterMovementOutput(@this.ExtractState(default, false), default);

    public static bool IsGrounded(this CharacterControllerState @this) =>
        @this.IsGrounded.IsTrue;

    public static bool HasBeenGrounded(this CharacterControllerState @this) =>
        @this.IsGrounded.IsTrueStay;

    public static bool HasJustLanded(this CharacterControllerState @this) =>
        @this.IsGrounded.IsTrueThisFrame;

    public static bool HasJustLeftGround(this CharacterControllerState @this) =>
        @this.IsGrounded.IsFalseThisFrame;

    static Func<RaycastHit, Option<Vector3>> ExtractNormal =
        raycastHit => raycastHit.normal.AsOption();

    /// <summary>
    /// returns current surface normal if hit was present or simply its relative up
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static Vector3 ReducedSurfaceNormal(this CharacterControllerState @this) =>
        @this
        .SurfaceTouched
        .Where(@this.IsGrounded())
        .Bind(ExtractNormal)
        .Reduce(@this.TransformData.Rotation * Vector3.up);
}
