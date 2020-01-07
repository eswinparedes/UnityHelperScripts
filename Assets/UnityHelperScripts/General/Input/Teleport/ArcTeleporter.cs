using UnityEngine;
using System.Linq;
using SUHScripts.Functional;
using System;
using static SUHScripts.ArcTeleporterSystem;

namespace SUHScripts
{
    using static MathHelper;

    [System.Serializable]
    public class ArcTeleporter
    {
        [Header("Teleporter Settings")]
        [SerializeField] ArcCaster m_arcCaster = default;
        [SerializeField] ArcCastLineRender m_arcCasterPresence = default;
        [Header("Target Dependencies")]
        [SerializeField] Transform m_teleportTarget = default; // target transferred by teleport
        [SerializeField] Transform m_yRotationOffset = default;

        bool displayActive = false;
        Option<RaycastHit> RaycastHitGroundOption;
        Func<RaycastHit, bool> HitValidator = hit => true;

        #region Internal Behaviour
        void OnArcCast(RaycastData data, Vector3[] positions)
        {
            RaycastHitGroundOption =
                data
                .RaycastHitOption
                .Where(HitValidator);

            RaycastHitGroundOption
                .Match(
                () => m_arcCasterPresence.OnInvalid(),
                hit => m_arcCasterPresence.OnValid(hit));

            m_arcCasterPresence.DrawArc(positions);
        }
        #endregion

        #region External Behaviour
        public void Start()
        {
            m_arcCaster.OnRaycastEvent += OnArcCast;
        }

        public void End()
        {
            m_arcCaster.OnRaycastEvent -= OnArcCast;
        }
        public void Update()
        {
            if (displayActive)
            {
                m_arcCaster.UpdateArc();
            }
        }

        public void RequestTeleport()
        {
            RaycastHitGroundOption
                .Where(displayActive)
                .ForEach(hit => GetRotationOffset(m_arcCasterPresence.PostitionMarker, m_yRotationOffset, m_teleportTarget).ApplyToTransform(m_teleportTarget));
        }

        public void ToggleDisplay(bool active)
        {
            displayActive = active;
            m_arcCasterPresence.ToggleDisplay(active);
        }

        public void PositionMarkerAxisInput(Vector2 input)
        {
            float angle = AngleDegreesFrom360(input.x, input.y);
            Vector3 normalForward = OrthoNormalVector(m_arcCaster.ArcIdentity.forward, m_arcCasterPresence.PostitionMarker.up);
            Vector3 angledForward = Quaternion.AngleAxis(angle, Vector3.up) * normalForward;

            m_arcCasterPresence.SetPositionMarkerRotation(angledForward);
        }
        #endregion
    }
}
