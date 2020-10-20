using UnityEngine;

namespace SUHScripts
{
    public struct ArcCalcSet
    {
        public Vector3 position;
        public Vector3 velocity;

        public ArcCalcSet(Vector3 position, Vector3 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }
    }
}
