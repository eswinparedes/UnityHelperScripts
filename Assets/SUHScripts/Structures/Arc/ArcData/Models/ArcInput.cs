using UnityEngine;

namespace SUHScripts
{
    public struct ArcInput
    {
        public readonly Vector3 ForwardVector;
        public readonly Vector3 RightVector;
        public readonly Vector3 Position;
        public readonly Vector3 Gravity;

        public ArcInput(Vector3 forward, Vector3 right, Vector3 position, Vector3 gravity)
        {
            ForwardVector = forward;
            RightVector = right;
            Position = position;
            Gravity = gravity;
        }
    }
}
