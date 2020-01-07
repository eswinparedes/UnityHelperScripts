using UnityEngine;


namespace SUHScripts
{
    using static MathHelper;

    [System.Serializable]
    public class ArcCastLineRender
    {
        [Header("Display Settings")]
        [SerializeField] LineRenderer m_arcRenderer = default;
        [SerializeField] GameObject m_positionMarker = default;
        [SerializeField] Material m_onValidHitMaterial = default;
        [SerializeField] Material m_onInvalidHitMaterial = default;

        public Transform PostitionMarker => m_positionMarker.transform;

        public void OnInvalid()
        {
            if (m_positionMarker.activeInHierarchy)
                m_positionMarker.SetActive(false);

            m_arcRenderer.material = m_onInvalidHitMaterial;
        }

        public void OnValid(RaycastHit hit)
        {
            if(!m_positionMarker.activeInHierarchy)
                m_positionMarker.SetActive(true);

            m_arcRenderer.material = m_onValidHitMaterial;

            m_positionMarker.transform.position = 
                hit.point + hit.normal * 0.1f;

            m_positionMarker.transform.up = hit.normal;
        }

        public void DrawArc(Vector3[] arcPositions)
        {
            m_arcRenderer.positionCount = arcPositions.Length;
            m_arcRenderer.SetPositions(arcPositions);
        }

        public void SetPositionMarkerRotation(Vector3 relativeForward)
        {
            m_positionMarker.transform.rotation =
                OrthoNormalRotation(relativeForward, m_positionMarker.transform.up);
        }

        public void ToggleDisplay(bool isActive)
        {
            m_arcRenderer.enabled = isActive;
            m_positionMarker.SetActive(isActive);
        }
    }

}
