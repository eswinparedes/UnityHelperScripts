using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class C_MouseInput : C_TouchInterfaceInput {

    SO_TouchInterfaceData m_touchInterfaceData;

    public C_MouseInput(SO_TouchInterfaceData touchInterfaceData)
    {
        m_touchInterfaceData = touchInterfaceData;
    }

    void TapControls()
    {
        m_touchInterfaceData.TapThisFrame.IsTrue = Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
        m_touchInterfaceData.TapHeld.IsTrue = Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0);
        m_touchInterfaceData.TapReleasedThisFrame.IsTrue = Input.GetMouseButtonUp(0);
        m_touchInterfaceData.TapPositionLastFrame.Value = m_touchInterfaceData.TapPositionThisFrame.Value;
        m_touchInterfaceData.TapPositionThisFrame.Value = Input.mousePosition;


        m_touchInterfaceData.SwipeMovementVector.Value = m_touchInterfaceData.TapPositionLastFrame.Value - m_touchInterfaceData.TapPositionThisFrame.Value;

        Vector2 screenWidthVector = m_touchInterfaceData.SwipeMovementVector.Value / Screen.width;
        m_touchInterfaceData.SwipeWidthsPerSecond.Value = screenWidthVector.magnitude / Time.deltaTime;

        m_touchInterfaceData.SwipeAngleThisFrame.Value = Vector2.SignedAngle(Vector2.right, m_touchInterfaceData.SwipeDirection.Value);
        m_touchInterfaceData.PinchDistanceDelta.Value = Input.GetAxis("Mouse ScrollWheel");
    }

    public override void UpdateControls()
    {
        TapControls();
    }
}
