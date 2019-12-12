using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zzTransformOrientation_TEST : MonoBehaviour
{
    [SerializeField] KeyCode applyOrientationKey = default;
    [SerializeField] Transform m_sourceOrientation = default;
    [SerializeField] Transform m_targetTransform = default;
    [SerializeField] Vector3 m_rotationOffset = default;
    [SerializeField] Vector3 m_positionOffset = default;
    [SerializeField] bool m_perUpdate = false;

    private void Update()
    {
        if (Input.GetKeyDown(applyOrientationKey) || m_perUpdate)
        {
            var orientation =
                m_sourceOrientation
                .ExtractOrientationOffset(m_positionOffset, Quaternion.Euler(m_rotationOffset))
                .ApplyToTransform(m_targetTransform);
        }
    }

}
