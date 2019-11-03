using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ScreenRaycaster : A_Component
{
    [SerializeField] Raycaster m_raycaster = default;
    [SerializeField] Camera m_camera = default;
    [SerializeField] SO_A_Vector3 m_screenPos = default;

    public override void Execute() =>
        m_raycaster.Raycast(m_camera, m_screenPos.Value);
}
