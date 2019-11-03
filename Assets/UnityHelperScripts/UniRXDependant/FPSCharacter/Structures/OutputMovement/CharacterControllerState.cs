using SUHScripts.Functional;
using UnityEngine;

[System.Serializable]
public struct CharacterControllerState
{
    public Vector3 Velocity { get; private set; }
    public TransformData TransformData { get; private set; }
    public BoolTrifecta IsGrounded { get; private set; }
    public Option<RaycastHit> SurfaceTouched { get; private set; }

    public CharacterControllerState(Vector3 velocity, TransformData tData, Option<RaycastHit> surfaceTouched, BoolTrifecta isGrounded)
    {
        this.Velocity = velocity;
        this.TransformData = tData;
        this.IsGrounded = isGrounded;
        this.SurfaceTouched = surfaceTouched;
    }
}