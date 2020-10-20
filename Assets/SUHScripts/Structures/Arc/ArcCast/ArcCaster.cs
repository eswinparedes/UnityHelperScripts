using SUHScripts.Functional;
using System;
using UnityEngine;
using System.Linq;

namespace SUHScripts
{
    [System.Serializable]
    public class ArcCaster 
    {
        [Header("Arc Sources")]
        [SerializeField] Transform m_arcIdentity = default;
        [Header("Settings")]
        [SerializeField] ArcSettings m_arcSettings = default;
        [SerializeField] LayerMask m_excludeLayers = default;

        public event Action<RaycastData, Vector3[]> OnRaycastEvent;

        public Transform ArcIdentity => m_arcIdentity;
        #region External Behaviour
        public void UpdateArc()
        {
            var arcCastOutput =
                m_arcIdentity
                .ExtractArcInput(Physics.gravity)
                .ArcCast(m_arcSettings, ~m_excludeLayers);

            var rayPositions = 
                    arcCastOutput
                    .points
                    .TakeLast(2);

            var p0 = rayPositions.First();
            var p1 = rayPositions.Last();
            var ray = new Ray(p0, p1 - p0);

            var hitOption =
                arcCastOutput
                .hitOption
                .Bind(hit => (Option<RaycastHit>) hit);

            OnRaycastEvent?.Invoke(new RaycastData(ray, hitOption), arcCastOutput.points);
        }
        #endregion
    }
}
