using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour
{
    [SerializeField] TouchInterfaceData m_touchInterfaceData = default;
    [SerializeField] Camera m_screenCastCamera = default;

    void TapControls()
    {
        m_touchInterfaceData.TapThisFrame = Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
        m_touchInterfaceData.TapHeld = Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0);
        m_touchInterfaceData.TapReleasedThisFrame = Input.GetMouseButtonUp(0);
        m_touchInterfaceData.TapPositionLastFrame = m_touchInterfaceData.TapPositionThisFrame;
        m_touchInterfaceData.TapPositionThisFrame = Input.mousePosition;


        m_touchInterfaceData.SwipeMovementVector = m_touchInterfaceData.TapPositionLastFrame - m_touchInterfaceData.TapPositionThisFrame;

        Vector2 screenWidthVector = m_touchInterfaceData.SwipeMovementVector / Screen.width;
        m_touchInterfaceData.SwipeWidthsPerSecond = screenWidthVector.magnitude / Time.deltaTime;

        m_touchInterfaceData.SwipeAngleThisFrame = Vector2.SignedAngle(Vector2.right, m_touchInterfaceData.SwipeDirection);
        m_touchInterfaceData.PinchDistanceDelta = Input.GetAxis("Mouse ScrollWheel");

        m_touchInterfaceData.TouchScreenRay = m_screenCastCamera.ScreenPointToRay(m_touchInterfaceData.TapPositionThisFrame);
    }

    void Update()
    {
        TapControls();
    }
    //update
}
