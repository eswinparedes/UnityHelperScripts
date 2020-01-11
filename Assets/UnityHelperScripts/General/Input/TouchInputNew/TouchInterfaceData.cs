using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "SUHS/Touch Interface Data")]
    public class TouchInterfaceData : ScriptableObject
    {
        public bool TapThisFrame;
        public bool TapHeld;
        public bool TapReleasedThisFrame;

        public float PinchDistanceDelta;
        public float PinchRotationDelta;

        public Vector3 InputWorldPosition;

        public Vector3 TapPositionThisFrame;
        public Vector3 TapPositionLastFrame;

        public Vector2 SwipeDirection;
        public Vector2 SwipeMovementVector;
        public float SwipeWidthsPerSecond;
        public float SwipeAngleThisFrame;

        public Ray TouchScreenRay;
    }
}

