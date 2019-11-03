using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ArcTeleporterTest : MonoBehaviour
{
    [SerializeField] FPSCameraBehaviour m_camBehaviour = default;
    [SerializeField] ArcTeleporter m_teleporter = default;

    private void OnEnable()
    {
        m_teleporter.Start();
    }

    private void OnDisable()
    {
        m_teleporter.End();
    }
    private void Update()
    {
        m_camBehaviour.Update(Time.deltaTime);
        m_teleporter.Update();

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        m_teleporter.PositionMarkerAxisInput(new Vector2(x, y));
    }

    public void SetTeleporterActive(bool isActive) =>
        m_teleporter.ToggleDisplay(isActive);

    public void Teleport() =>
        m_teleporter.RequestTeleport();
}
