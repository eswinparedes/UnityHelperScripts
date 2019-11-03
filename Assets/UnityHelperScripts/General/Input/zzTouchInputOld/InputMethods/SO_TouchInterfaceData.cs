using UnityEngine;

[CreateAssetMenu(menuName = "Input/Touch Input/Basic Touch Input Data")]
public class SO_TouchInterfaceData : ScriptableObject {

    public SO_A_BoolReadWrite TapThisFrame;
    public SO_A_BoolReadWrite TapHeld;
    public SO_A_BoolReadWrite TapReleasedThisFrame;
  
    public SO_A_FloatReadWrite PinchDistanceDelta;
    public SO_A_FloatReadWrite PinchRotationDelta;
 
    public SO_A_Vector3ReadWrite InputWorldPosition;

    public SO_A_Vector3ReadWrite TapPositionThisFrame;
    public SO_A_Vector3ReadWrite TapPositionLastFrame;

    public SO_A_Vector2ReadWrite SwipeDirection;
    public SO_A_Vector2ReadWrite SwipeMovementVector;
    public SO_A_FloatReadWrite SwipeWidthsPerSecond;
    public SO_A_FloatReadWrite SwipeAngleThisFrame ;

    public SO_A_RayReadWrite TouchScreenRay;
}
