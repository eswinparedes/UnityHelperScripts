using UnityEngine;

namespace SUHScripts
{
    [System.Obsolete]
    [System.Serializable]
    public struct TransformData
    {
        public Vector3 Position { get; private set; }
        public Vector3 Scale { get; private set; }
        public Quaternion Rotation { get; private set; }

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rotation;
        }

        public TransformData(Vector3 position, Quaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = Vector3.one;
        }
    }
}

