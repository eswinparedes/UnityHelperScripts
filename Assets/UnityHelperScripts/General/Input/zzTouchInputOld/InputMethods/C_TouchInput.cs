using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class C_TouchInput : C_TouchInterfaceInput
{
    SO_TouchInterfaceData m_touchInterfaceData;

    public C_TouchInput(SO_TouchInterfaceData touchInterfaceData)
    {
        m_touchInterfaceData = touchInterfaceData;
    }

    public override void UpdateControls()
    {
        InitTap();
        GetInput();
        GetPinchValue();
    }
    //update

    #region Get Input
    private void GetInput()
    {
        //No check if second  touch hits Gui object
        m_touchInterfaceData.TapThisFrame.IsTrue = false;
        m_touchInterfaceData.TapThisFrame.IsTrue = false;

        if (Input.touchCount > 0)
        {
            GetSingleTapInput();
        }
    }

    public void GetSingleTapInput()
    {
        Touch touch = Input.GetTouch(0);
   
        switch (touch.phase)
        {
            case TouchPhase.Began: OnTouchBegan(touch); break;
            case TouchPhase.Moved: OnTouchMoved(touch); break;
            case TouchPhase.Ended: OnTouchEnded(touch); break;
            case TouchPhase.Stationary: OnTouchMoved(touch); break;
            case TouchPhase.Canceled: OnTouchEnded(touch); break;
        }
    }

    void InitTap()
    {
        m_touchInterfaceData.TapThisFrame.IsTrue = false;
        m_touchInterfaceData.TapReleasedThisFrame.IsTrue = false;
    }
    public void OnTouchBegan(Touch touch)
    {
        m_touchInterfaceData.TapThisFrame.IsTrue = !EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        m_touchInterfaceData.TapPositionThisFrame.Value = touch.position;
        m_touchInterfaceData.TapReleasedThisFrame.IsTrue = false;
    }

    public void OnTouchMoved(Touch touch)
    {
        m_touchInterfaceData.TapHeld.IsTrue = true;
        m_touchInterfaceData.SwipeMovementVector.Value = touch.deltaPosition;
        m_touchInterfaceData.TapPositionThisFrame.Value = touch.position;
        m_touchInterfaceData.TapPositionLastFrame.Value = touch.position - touch.deltaPosition;
        m_touchInterfaceData.SwipeDirection.Value = m_touchInterfaceData.SwipeMovementVector.Value.normalized;
        m_touchInterfaceData.TapReleasedThisFrame.IsTrue = false;

        SetSwipeAcceleration(touch);
    }

    void SetSwipeAcceleration(Touch touch)
    {
        Vector2 screenWidthVector = m_touchInterfaceData.SwipeMovementVector.Value / Screen.width;
        m_touchInterfaceData.SwipeWidthsPerSecond.Value = screenWidthVector.magnitude / touch.deltaTime;

        m_touchInterfaceData.SwipeAngleThisFrame.Value = Vector2.SignedAngle(Vector2.right, m_touchInterfaceData.SwipeDirection.Value);
    }

    public void OnTouchEnded(Touch touch)
    {
        m_touchInterfaceData.TapReleasedThisFrame.IsTrue = true;
        m_touchInterfaceData.TapHeld.IsTrue = false;
        
        m_touchInterfaceData.TapPositionThisFrame.Value = touch.position;
        m_touchInterfaceData.TapPositionLastFrame.Value = touch.position - touch.deltaPosition;
    }

    private void GetPinchValue()
    {
        float pinchTouch = 0;
        float pinchRotate = 0;
        if (Input.touchCount == 2)
        {
            Touch touch_0 = Input.GetTouch(0);
            Touch touch_1 = Input.GetTouch(1);

            Vector2 touch_0_previousPos = touch_0.position - touch_0.deltaPosition;
            Vector2 touch_1_previousPos = touch_1.position - touch_1.deltaPosition;

            //Get Touch distance change
            float previousTouchDeltaMag = (touch_0_previousPos - touch_1_previousPos).magnitude;
            float touchDeltaMag = (touch_0.position - touch_1.position).magnitude;


            pinchTouch = previousTouchDeltaMag - touchDeltaMag;

            //Get Touch Rotation change
            //INIT?
            float pinchTurnRatio = Mathf.PI / 2;
            float minTurnAngle = 0;

            float turnAngle = Angle(touch_0.position, touch_1.position);
            float prevTurn = Angle(touch_0.position - touch_0.deltaPosition,
                                   touch_1.position - touch_1.deltaPosition);
            float turnAngleDelta = Mathf.DeltaAngle(prevTurn, turnAngle);

            // Turning Angle Threshold
            if (Mathf.Abs(turnAngleDelta) > minTurnAngle)
            {
                turnAngleDelta *= pinchTurnRatio;
            }
            else
            {
                turnAngle = turnAngleDelta = 0;
            }

            pinchRotate = -turnAngleDelta;
        }
        else
        {

        }

        m_touchInterfaceData.PinchDistanceDelta.Value =  pinchTouch * .001f;
        m_touchInterfaceData.PinchRotationDelta.Value = pinchRotate;
    }

    static private float Angle(Vector2 pos1, Vector2 pos2)
    {
        Vector2 from = pos2 - pos1;
        Vector2 to = new Vector2(1, 0);

        float result = Vector2.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        if (cross.z > 0)
        {
            result = 360f - result;
        }

        return result;
    }

    private void GetPinchRotation()
    {

    }
    #endregion

    #region Utility
    public Quaternion GetLookToMouseRotation(Transform _transform)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(_transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg + Camera.main.transform.rotation.eulerAngles.z;

        return Quaternion.Euler(new Vector3(_transform.rotation.x, _transform.rotation.y, angle));
    }
    #endregion
}
