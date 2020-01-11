using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace SUHScripts
{
    public class M_CustomInputModule : UnityEngine.EventSystems.PointerInputModule
    {
        [SerializeField] A_CustomInputModuleSettings m_inputModuleSettings = default;

        // The raycaster that gets to do pointer interaction (e.g. with a mouse), gaze interaction always works
        // private OVRRaycaster _activeGraphicRaycaster;
        [NonSerialized]
        public CustomGraphicRaycaster activeGraphicRaycaster;
        [Header("Dragging")]
        [Tooltip("Minimum pointer movement in degrees to start dragging")]
        public float angleDragThreshold = 1;

        #region StandaloneInputModule code

        private float m_NextAction;

        protected M_CustomInputModule()
        { }

        protected new void Reset()
        {
            allowActivationOnMobileDevice = true;
        }

        [SerializeField]
        private float m_InputActionsPerSecond = 10;

        [SerializeField]
        private bool m_AllowActivationOnMobileDevice;

        public bool allowActivationOnMobileDevice
        {
            get { return m_AllowActivationOnMobileDevice; }
            set { m_AllowActivationOnMobileDevice = value; }
        }

        public float inputActionsPerSecond
        {
            get { return m_InputActionsPerSecond; }
            set { m_InputActionsPerSecond = value; }
        }

        public override bool ShouldActivateModule()
        {
            if (!base.ShouldActivateModule())
                return false;

            var shouldActivate = m_inputModuleSettings.SubmitCondition;
            shouldActivate |= m_inputModuleSettings.CancelCondition;
            shouldActivate |= !Mathf.Approximately(m_inputModuleSettings.HorizontalAxis, 0.0f);
            shouldActivate |= !Mathf.Approximately(m_inputModuleSettings.VerticalAxis, 0.0f);

            return shouldActivate;
        }

        public override void ActivateModule()
        {
            base.ActivateModule();

            var toSelect = eventSystem.currentSelectedGameObject;
            if (toSelect == null)
                toSelect = eventSystem.firstSelectedGameObject;

            eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
        }

        public override void DeactivateModule()
        {
            base.DeactivateModule();
            ClearSelection();
        }
        /// <summary>
        /// Process submit keys.
        /// </summary>
        private bool SendSubmitEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;

            var data = GetBaseEventData();
            if (m_inputModuleSettings.SubmitCondition)
                UnityEngine.EventSystems.ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, UnityEngine.EventSystems.ExecuteEvents.submitHandler);

            if (m_inputModuleSettings.CancelCondition)
                UnityEngine.EventSystems.ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, UnityEngine.EventSystems.ExecuteEvents.cancelHandler);
            return data.used;
        }

        #region Move
        private bool AllowMoveEventProcessing(float time)
        {
            bool allow = m_inputModuleSettings.HorizontalAxisThisFrame;
            allow |= m_inputModuleSettings.VerticalAxisThisFrame;
            allow |= (time > m_NextAction);
            return allow;
        }

        private Vector2 GetRawMoveVector()
        {
            Vector2 move = Vector2.zero;
            move.x = m_inputModuleSettings.HorizontalAxis;
            move.y = m_inputModuleSettings.VerticalAxis;

            if (m_inputModuleSettings.HorizontalAxisThisFrame)
            {
                if (move.x < 0)
                    move.x = -1f;
                if (move.x > 0)
                    move.x = 1f;
            }
            if (m_inputModuleSettings.VerticalAxisThisFrame)
            {
                if (move.y < 0)
                    move.y = -1f;
                if (move.y > 0)
                    move.y = 1f;
            }
            return move;
        }

        /// <summary>
        /// Process keyboard events.
        /// </summary>
        private bool SendMoveEventToSelectedObject()
        {
            float time = Time.unscaledTime;

            if (!AllowMoveEventProcessing(time))
                return false;

            Vector2 movement = GetRawMoveVector();
            // Debug.Log(m_ProcessingEvent.rawType + " axis:" + m_AllowAxisEvents + " value:" + "(" + x + "," + y + ")");
            var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
            if (!Mathf.Approximately(axisEventData.moveVector.x, 0f)
                || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
            {
                UnityEngine.EventSystems.ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, UnityEngine.EventSystems.ExecuteEvents.moveHandler);
            }
            m_NextAction = time + 1f / m_InputActionsPerSecond;
            return axisEventData.used;
        }
        #endregion

        private bool SendUpdateEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;

            var data = GetBaseEventData();
            UnityEngine.EventSystems.ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, UnityEngine.EventSystems.ExecuteEvents.updateSelectedHandler);
            return data.used;
        }

        /// <summary>
        /// Process the current mouse press.
        /// </summary>
        private void ProcessMousePress(MouseButtonEventData data)
        {
            var pointerEvent = data.buttonData;
            var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;
        
            // PointerDown notification
            if (data.PressedThisFrame())
            {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

                DeselectIfSelectionChanged(currentOverGo, pointerEvent);

                // search for the control that will receive the press
                // if we can't find a press handler set the press
                // handler to be what would receive a click.
                var newPressed = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

                // didnt find a press handler... search for a click handler
                if (newPressed == null)
                    newPressed = UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<UnityEngine.EventSystems.IPointerClickHandler>(currentOverGo);

                // Debug.Log("Pressed: " + newPressed);

                float time = Time.unscaledTime;

                if (newPressed == pointerEvent.lastPress)
                {
                    var diffTime = time - pointerEvent.clickTime;
                    if (diffTime < 0.3f)
                        ++pointerEvent.clickCount;
                    else
                        pointerEvent.clickCount = 1;

                    pointerEvent.clickTime = time;
                }
                else
                {
                    pointerEvent.clickCount = 1;
                }

                pointerEvent.pointerPress = newPressed;
                pointerEvent.rawPointerPress = currentOverGo;

                pointerEvent.clickTime = time;

                // Save the drag handler as well
                pointerEvent.pointerDrag = UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<UnityEngine.EventSystems.IDragHandler>(currentOverGo);

                if (pointerEvent.pointerDrag != null)
                    UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
            }

            // PointerUp notification
            if (data.ReleasedThisFrame())
            {
                UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);

                // see if we mouse up on the same element that we clicked on...
                var pointerUpHandler = UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<UnityEngine.EventSystems.IPointerClickHandler>(currentOverGo);

                // PointerClick and Drop events
                if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
                {
                    UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
                }
                else if (pointerEvent.pointerDrag != null)
                {
                    UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.dropHandler);
                }

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;

                if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                    UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.endDragHandler);

                pointerEvent.dragging = false;
                pointerEvent.pointerDrag = null;

                // redo pointer enter / exit to refresh state
                // so that if we moused over somethign that ignored it before
                // due to having pressed on something else
                // it now gets it.
                if (currentOverGo != pointerEvent.pointerEnter)
                {
                    HandlePointerExitAndEnter(pointerEvent, null);
                    HandlePointerExitAndEnter(pointerEvent, currentOverGo);
                }
            }
        }
        #endregion
        #region Modified StandaloneInputModule methods

        /// <summary>
        /// Takes in the emulated MouseState 'mouseData' and processes it, 'mouseData' can be
        /// emulated from vr input
        /// formerly: ProcessMouseEvent
        /// </summary>
        private void ProcessMouseEvent(MouseState mouseData)
        {
            var pressed = mouseData.AnyPressesThisFrame();
            var released = mouseData.AnyReleasesThisFrame();

            var leftButtonData = mouseData.GetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Left).eventData;

            // Process the first mouse button fully
            ProcessMousePress(leftButtonData);
            ProcessMove(leftButtonData.buttonData);
            ProcessDrag(leftButtonData.buttonData);

            if (!Mathf.Approximately(leftButtonData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
            {
                var scrollHandler = UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<UnityEngine.EventSystems.IScrollHandler>(leftButtonData.buttonData.pointerCurrentRaycast.gameObject);
                UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(scrollHandler, leftButtonData.buttonData, UnityEngine.EventSystems.ExecuteEvents.scrollHandler);
            }
        }

        /// <summary>
        /// Process this InputModule. Same as the StandaloneInputModule version, except that it calls
        /// ProcessMouseEvent twice, once for gaze pointers, and once for mouse pointers.
        /// </summary>
        public override void Process()
        {
            bool usedEvent = SendUpdateEventToSelectedObject();

            if (eventSystem.sendNavigationEvents)
            {
                if (!usedEvent)
                    usedEvent |= SendMoveEventToSelectedObject();

                if (!usedEvent)
                    SendSubmitEventToSelectedObject();
            }

            ProcessMouseEvent(GetWorldSpaceControlledPointerData());

            m_inputModuleSettings.InvokeOnInputModuleProcessComplete();
        }

        #endregion

        #region PointerEventData pool
        /// <summary>
        /// Clear pointer state for both types of pointer
        /// </summary>
        protected new void ClearSelection()
        {
            var baseEventData = GetBaseEventData();
            HandlePointerExitAndEnter(m_customRayPointerEventData, null);
            eventSystem.SetSelectedGameObject(null, baseEventData);
        }
        #endregion

        /// <summary>
        /// For RectTransform, calculate it's normal in world space
        /// </summary>
        static Vector3 GetRectTransformNormal(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            Vector3 BottomEdge = corners[3] - corners[0];
            Vector3 LeftEdge = corners[1] - corners[0];
            rectTransform.GetWorldCorners(corners);
            return Vector3.Cross(LeftEdge, BottomEdge).normalized;
        }

        private readonly MouseState m_MouseState = new MouseState();

        CustomRayPointerEventData m_customRayPointerEventData;

        // The following 2 functions are equivalent to PointerInputModule.GetMousePointerEventData but are customized to
        // get data for ray pointers and canvas mouse pointers.
        /// <summary>
        /// Get Mouse State for world space ray
        /// </summary>
        /// <returns></returns>
        protected MouseState GetWorldSpaceControlledPointerData()
        {
            m_customRayPointerEventData = m_customRayPointerEventData??
                new CustomRayPointerEventData(eventSystem)
                { pointerId=  kMouseLeftId};

            m_customRayPointerEventData.Reset();

            //This ray will be used for testing against canvas
            m_customRayPointerEventData.worldSpaceRay = m_inputModuleSettings.InputRay;

            m_customRayPointerEventData.button = UnityEngine.EventSystems.PointerEventData.InputButton.Left;
            m_customRayPointerEventData.useDragThreshold = true;

            // Perform raycast to find intersections with world
            eventSystem.RaycastAll(m_customRayPointerEventData, m_RaycastResultCache);
            RaycastResult raycast = FindFirstRaycast(m_RaycastResultCache);
            raycast = m_inputModuleSettings.InvokeGetProcessedResult(raycast);
            //ADDED
            m_inputModuleSettings.InputRaycastResult = new List<RaycastResult>(m_RaycastResultCache);

            m_customRayPointerEventData.pointerCurrentRaycast = raycast;
            m_RaycastResultCache.Clear();

            CustomGraphicRaycaster graphicsRaycaster = raycast.module as CustomGraphicRaycaster;
            // We're only interested in intersections from graphicsRaycasters
            if (graphicsRaycaster)
            {
                // The Unity UI system expects event data to have a screen position
                // so even though this raycast came from a world space ray we must get a screen
                // space position for the camera attached to this raycaster for compatability
                m_customRayPointerEventData.position = graphicsRaycaster.GetScreenPosition(raycast);
            }

            m_MouseState.SetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Left, GetPointerButtonFrameState(), m_customRayPointerEventData);

            return m_MouseState;
        }
        /// <summary>
        /// New version of ShouldStartDrag implemented first in PointerInputModule. This version differs in that
        /// for ray based pointers it makes a decision about whether a drag should start based on the angular change
        /// the pointer has made so far, as seen from the camera. This also works when the world space ray is 
        /// translated rather than rotated, since the beginning and end of the movement are considered as angle from
        /// the same point.
        /// </summary>
        private bool ShouldStartDrag(UnityEngine.EventSystems.PointerEventData pointerEvent)
        {
            if (!pointerEvent.useDragThreshold)
                return true;

            if (pointerEvent as CustomRayPointerEventData == null)
            {
                // Same as original behaviour for canvas based pointers
                return (pointerEvent.pressPosition - pointerEvent.position).sqrMagnitude >= eventSystem.pixelDragThreshold * eventSystem.pixelDragThreshold;
            }
            else
            {
                // When it's not a screen space pointer we have to look at the angle it moved rather than the pixels distance
                // For gaze based pointing screen-space distance moved will always be near 0
                Vector3 cameraPos = pointerEvent.pressEventCamera.transform.position;
                Vector3 pressDir = (pointerEvent.pointerPressRaycast.worldPosition - cameraPos).normalized;
                Vector3 currentDir = (pointerEvent.pointerCurrentRaycast.worldPosition - cameraPos).normalized;
                return Vector3.Dot(pressDir, currentDir) < Mathf.Cos(Mathf.Deg2Rad * (angleDragThreshold));
            }
        }

        protected override void ProcessDrag(UnityEngine.EventSystems.PointerEventData pointerEvent)
        {
            //we'll assume we determined pointer is moving already
            //bool moving = IsPointerMoving(pointerEvent);
            if (pointerEvent.pointerDrag != null
                && !pointerEvent.dragging
                && ShouldStartDrag(pointerEvent))
            {
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
                pointerEvent.dragging = true;
            }

            // Drag notification
            if (pointerEvent.dragging && pointerEvent.pointerDrag != null)
            {
                // Before doing drag we should cancel any pointer down state
                // And clear selection!
                if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
                {
                    UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                    pointerEvent.eligibleForClick = false;
                    pointerEvent.pointerPress = null;
                    pointerEvent.rawPointerPress = null;
                }
                UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
            }
        }

        /// <summary>
        /// Get pressed/released state of conditions and return corresponding pointerevent data framePressState
        /// formerly: GetGazeButtonState()
        /// </summary>
        /// <returns></returns>
        protected UnityEngine.EventSystems.PointerEventData.FramePressState GetPointerButtonFrameState()
        {

            if (m_inputModuleSettings.PressedCondition && m_inputModuleSettings.ReleasedCondition)
            {
                return UnityEngine.EventSystems.PointerEventData.FramePressState.PressedAndReleased;
            }
            if (m_inputModuleSettings.PressedCondition)
            {
                return UnityEngine.EventSystems.PointerEventData.FramePressState.Pressed;
            }
            if (m_inputModuleSettings.ReleasedCondition)
            {
                return UnityEngine.EventSystems.PointerEventData.FramePressState.Released;
            }

            return UnityEngine.EventSystems.PointerEventData.FramePressState.NotChanged;
        }
}


    /*Original--------------------------------------------------
    [SerializeField] A_CustomInputModuleSettings m_inputModuleSettings;

    // The raycaster that gets to do pointer interaction (e.g. with a mouse), gaze interaction always works
    // private OVRRaycaster _activeGraphicRaycaster;
    [NonSerialized]
    public CustomGraphicRaycaster activeGraphicRaycaster;
    [Header("Dragging")]
    [Tooltip("Minimum pointer movement in degrees to start dragging")]
    public float angleDragThreshold = 1;

    // The following region contains code exactly the same as the implementation
    // of StandaloneInputModule. It is copied here rather than inheriting from StandaloneInputModule
    // because most of StandaloneInputModule is private so it isn't possible to easily derive from.
    // Future changes from Unity to StandaloneInputModule will make it possible for this class to
    // derive from StandaloneInputModule instead of PointerInput module.
    // 
    // The following functions are not present in the following region since they have modified
    // versions in the next region:
    // Process
    // ProcessMouseEvent
    // UseMouse
    #region StandaloneInputModule code

    private float m_NextAction;

    private Vector2 m_LastMousePosition;
    private Vector2 m_MousePosition;

    protected M_CustomInputModule()
    { }

    protected new void Reset()
    {
        allowActivationOnMobileDevice = true;
    }

    [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
    public enum InputMode
    {
        Mouse,
        Buttons
    }

    [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
    public InputMode inputMode
    {
        get { return InputMode.Mouse; }
    }

    [SerializeField]
    private float m_InputActionsPerSecond = 10;

    [SerializeField]
    private bool m_AllowActivationOnMobileDevice;

    public bool allowActivationOnMobileDevice
    {
        get { return m_AllowActivationOnMobileDevice; }
        set { m_AllowActivationOnMobileDevice = value; }
    }

    public float inputActionsPerSecond
    {
        get { return m_InputActionsPerSecond; }
        set { m_InputActionsPerSecond = value; }
    }

    public override void UpdateModule()
    {
        m_LastMousePosition = m_MousePosition;
        m_MousePosition = Input.mousePosition;
    }

    public override bool ShouldActivateModule()
    {
        if (!base.ShouldActivateModule())
            return false;

        var shouldActivate = m_inputModuleSettings.SubmitCondition;
        shouldActivate |= m_inputModuleSettings.CancelCondition;
        shouldActivate |= !Mathf.Approximately(m_inputModuleSettings.HorizontalAxis, 0.0f);
        shouldActivate |= !Mathf.Approximately(m_inputModuleSettings.VerticalAxis, 0.0f);
        shouldActivate |= (m_MousePosition - m_LastMousePosition).sqrMagnitude > 0.0f;
        shouldActivate |= Input.GetMouseButtonDown(0);
        return shouldActivate;
    }

    public override void ActivateModule()
    {
        base.ActivateModule();
        m_MousePosition = Input.mousePosition;
        m_LastMousePosition = Input.mousePosition;

        var toSelect = eventSystem.currentSelectedGameObject;
        if (toSelect == null)
            toSelect = eventSystem.firstSelectedGameObject;

        eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
    }

    public override void DeactivateModule()
    {
        base.DeactivateModule();
        ClearSelection();
    }



    /// <summary>
    /// Process submit keys.
    /// </summary>
    private bool SendSubmitEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        if (m_inputModuleSettings.SubmitCondition)
            UnityEngine.EventSystems.ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, UnityEngine.EventSystems.ExecuteEvents.submitHandler);

        if (m_inputModuleSettings.CancelCondition)
            UnityEngine.EventSystems.ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, UnityEngine.EventSystems.ExecuteEvents.cancelHandler);
        return data.used;
    }

    private bool AllowMoveEventProcessing(float time)
    {
        bool allow = m_inputModuleSettings.HorizontalAxisThisFrame;
        allow |= m_inputModuleSettings.VerticalAxisThisFrame;
        allow |= (time > m_NextAction);
        return allow;
    }

    private Vector2 GetRawMoveVector()
    {
        Vector2 move = Vector2.zero;
        move.x = m_inputModuleSettings.HorizontalAxis;
        move.y = m_inputModuleSettings.VerticalAxis;

        if (m_inputModuleSettings.HorizontalAxisThisFrame)
        {
            if (move.x < 0)
                move.x = -1f;
            if (move.x > 0)
                move.x = 1f;
        }
        if (m_inputModuleSettings.VerticalAxisThisFrame)
        {
            if (move.y < 0)
                move.y = -1f;
            if (move.y > 0)
                move.y = 1f;
        }
        return move;
    }

    /// <summary>
    /// Process keyboard events.
    /// </summary>
    private bool SendMoveEventToSelectedObject()
    {
        float time = Time.unscaledTime;

        if (!AllowMoveEventProcessing(time))
            return false;

        Vector2 movement = GetRawMoveVector();
        // Debug.Log(m_ProcessingEvent.rawType + " axis:" + m_AllowAxisEvents + " value:" + "(" + x + "," + y + ")");
        var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
        if (!Mathf.Approximately(axisEventData.moveVector.x, 0f)
            || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
        {
            UnityEngine.EventSystems.ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, UnityEngine.EventSystems.ExecuteEvents.moveHandler);
        }
        m_NextAction = time + 1f / m_InputActionsPerSecond;
        return axisEventData.used;
    }

    private bool SendUpdateEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        UnityEngine.EventSystems.ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, UnityEngine.EventSystems.ExecuteEvents.updateSelectedHandler);
        return data.used;
    }

    /// <summary>
    /// Process the current mouse press.
    /// </summary>
    private void ProcessMousePress(MouseButtonEventData data)
    {
        var pointerEvent = data.buttonData;
        var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

        // PointerDown notification
        if (data.PressedThisFrame())
        {
            pointerEvent.eligibleForClick = true;
            pointerEvent.delta = Vector2.zero;
            pointerEvent.dragging = false;
            pointerEvent.useDragThreshold = true;
            pointerEvent.pressPosition = pointerEvent.position;
            pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

            DeselectIfSelectionChanged(currentOverGo, pointerEvent);

            // search for the control that will receive the press
            // if we can't find a press handler set the press
            // handler to be what would receive a click.
            var newPressed = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);

            // didnt find a press handler... search for a click handler
            if (newPressed == null)
                newPressed = UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<UnityEngine.EventSystems.IPointerClickHandler>(currentOverGo);

            // Debug.Log("Pressed: " + newPressed);

            float time = Time.unscaledTime;

            if (newPressed == pointerEvent.lastPress)
            {
                var diffTime = time - pointerEvent.clickTime;
                if (diffTime < 0.3f)
                    ++pointerEvent.clickCount;
                else
                    pointerEvent.clickCount = 1;

                pointerEvent.clickTime = time;
            }
            else
            {
                pointerEvent.clickCount = 1;
            }

            pointerEvent.pointerPress = newPressed;
            pointerEvent.rawPointerPress = currentOverGo;

            pointerEvent.clickTime = time;

            // Save the drag handler as well
            pointerEvent.pointerDrag = UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<UnityEngine.EventSystems.IDragHandler>(currentOverGo);

            if (pointerEvent.pointerDrag != null)
                UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
        }

        // PointerUp notification
        if (data.ReleasedThisFrame())
        {
            // Debug.Log("Executing pressup on: " + pointer.pointerPress);
            UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);

            // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

            // see if we mouse up on the same element that we clicked on...
            var pointerUpHandler = UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<UnityEngine.EventSystems.IPointerClickHandler>(currentOverGo);

            // PointerClick and Drop events
            if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
            {
                UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
            }
            else if (pointerEvent.pointerDrag != null)
            {
                UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.dropHandler);
            }

            pointerEvent.eligibleForClick = false;
            pointerEvent.pointerPress = null;
            pointerEvent.rawPointerPress = null;

            if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.endDragHandler);

            pointerEvent.dragging = false;
            pointerEvent.pointerDrag = null;

            // redo pointer enter / exit to refresh state
            // so that if we moused over somethign that ignored it before
            // due to having pressed on something else
            // it now gets it.
            if (currentOverGo != pointerEvent.pointerEnter)
            {
                HandlePointerExitAndEnter(pointerEvent, null);
                HandlePointerExitAndEnter(pointerEvent, currentOverGo);
            }
        }
    }
    #endregion
    #region Modified StandaloneInputModule methods

    /// <summary>
    /// Takes in the emulated MouseState 'mouseData' and processes it, 'mouseData' can be
    /// emulated from vr input
    /// formerly: ProcessMouseEvent
    /// </summary>
    private void ProcessMouseEvent(MouseState mouseData)
    {
        var pressed = mouseData.AnyPressesThisFrame();
        var released = mouseData.AnyReleasesThisFrame();

        var leftButtonData = mouseData.GetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Left).eventData;

        // Process the first mouse button fully
        ProcessMousePress(leftButtonData);
        ProcessMove(leftButtonData.buttonData);
        ProcessDrag(leftButtonData.buttonData);

        // Now process right / middle clicks
        ProcessMousePress(mouseData.GetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Right).eventData);
        ProcessDrag(mouseData.GetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Right).eventData.buttonData);
        ProcessMousePress(mouseData.GetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Middle).eventData);
        ProcessDrag(mouseData.GetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Middle).eventData.buttonData);

        if (!Mathf.Approximately(leftButtonData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
        {
            var scrollHandler = UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<UnityEngine.EventSystems.IScrollHandler>(leftButtonData.buttonData.pointerCurrentRaycast.gameObject);
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(scrollHandler, leftButtonData.buttonData, UnityEngine.EventSystems.ExecuteEvents.scrollHandler);
        }
    }

    /// <summary>
    /// Process this InputModule. Same as the StandaloneInputModule version, except that it calls
    /// ProcessMouseEvent twice, once for gaze pointers, and once for mouse pointers.
    /// </summary>
    public override void Process()
    {
        bool usedEvent = SendUpdateEventToSelectedObject();

        if (eventSystem.sendNavigationEvents)
        {
            if (!usedEvent)
                usedEvent |= SendMoveEventToSelectedObject();

            if (!usedEvent)
                SendSubmitEventToSelectedObject();
        }

        ProcessMouseEvent(GetWorldSpaceControlledPointerData());
#if !UNITY_ANDROID
            ProcessMouseEvent(GetCanvasPointerData());
#endif
    }

    #endregion


    /// <summary>
    /// Convenience function for cloning PointerEventData
    /// </summary>
    /// <param name="from">Copy this value</param>
    /// <param name="to">to this object</param>
    protected void CopyFromTo(CustomRayPointerEventData @from, CustomRayPointerEventData @to)
    {
        @to.position = @from.position;
        @to.delta = @from.delta;
        @to.scrollDelta = @from.scrollDelta;
        @to.pointerCurrentRaycast = @from.pointerCurrentRaycast;
        @to.pointerEnter = @from.pointerEnter;
        @to.worldSpaceRay = @from.worldSpaceRay;
    }
    /// <summary>
    /// Convenience function for cloning PointerEventData
    /// </summary>
    /// <param name="from">Copy this value</param>
    /// <param name="to">to this object</param>
    protected new void CopyFromTo(UnityEngine.EventSystems.PointerEventData @from, UnityEngine.EventSystems.PointerEventData @to)
    {
        @to.position = @from.position;
        @to.delta = @from.delta;
        @to.scrollDelta = @from.scrollDelta;
        @to.pointerCurrentRaycast = @from.pointerCurrentRaycast;
        @to.pointerEnter = @from.pointerEnter;
    }


    // In the following region we extend the PointerEventData system implemented in PointerInputModule
    // We define an additional dictionary for ray(e.g. gaze) based pointers. Mouse pointers still use the dictionary
    // in PointerInputModule
    #region PointerEventData pool

    // Pool for OVRRayPointerEventData for ray based pointers
    protected Dictionary<int, CustomRayPointerEventData> m_VRRayPointerData = new Dictionary<int, CustomRayPointerEventData>();

    /// <summary>
    /// Given 'id' and 'data', sets 'data' to default if the key 'id' does not exist within the dictionary
    /// variable: m_VRRayPointerData, otherwise, it returns the value associated with Key: 'id'
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <param name="create"></param>
    /// <returns></returns>
    protected bool GetPointerData(int id, out CustomRayPointerEventData data, bool create)
    {
        if (!m_VRRayPointerData.TryGetValue(id, out data) && create)
        {
            data = new CustomRayPointerEventData(eventSystem)
            {
                pointerId = id,
            };

            m_VRRayPointerData.Add(id, data);
            return true;
        }
        return false;
    }


    /// <summary>
    /// Clear pointer state for both types of pointer
    /// </summary>
    protected new void ClearSelection()
    {
        var baseEventData = GetBaseEventData();

        foreach (var pointer in m_PointerData.Values)
        {
            // clear all selection
            HandlePointerExitAndEnter(pointer, null);
        }
        foreach (var pointer in m_VRRayPointerData.Values)
        {
            // clear all selection
            HandlePointerExitAndEnter(pointer, null);
        }

        m_PointerData.Clear();
        eventSystem.SetSelectedGameObject(null, baseEventData);
    }
    #endregion

    /// <summary>
    /// For RectTransform, calculate it's normal in world space
    /// </summary>
    static Vector3 GetRectTransformNormal(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 BottomEdge = corners[3] - corners[0];
        Vector3 LeftEdge = corners[1] - corners[0];
        rectTransform.GetWorldCorners(corners);
        return Vector3.Cross(LeftEdge, BottomEdge).normalized;
    }

    private readonly MouseState m_MouseState = new MouseState();
    // Overridden so that we can process the two types of pointer separately


    // The following 2 functions are equivalent to PointerInputModule.GetMousePointerEventData but are customized to
    // get data for ray pointers and canvas mouse pointers.

    /// <summary>
    /// State for a pointer controlled by a world space ray. E.g. gaze pointer
    /// formerly: GetGazePointerData
    /// </summary>
    /// <returns></returns>
    protected MouseState GetWorldSpaceControlledPointerData()
    {
        // Check if m_VRRayPointerData has a value for kMouseLeftId, 
        //if it does, it sets leftdata to it otherwise, returns a new one
        CustomRayPointerEventData leftData;
        GetPointerData(kMouseLeftId, out leftData, true);
        leftData.Reset();

        //This ray will be used for testing against canvas
        leftData.worldSpaceRay = m_inputModuleSettings.InputRay; 

        //Populate some default values
        leftData.button = UnityEngine.EventSystems.PointerEventData.InputButton.Left;
        leftData.useDragThreshold = true;
        // Perform raycast to find intersections with world
        eventSystem.RaycastAll(leftData, m_RaycastResultCache);
        var raycast = FindFirstRaycast(m_RaycastResultCache);
        leftData.pointerCurrentRaycast = raycast;
        m_RaycastResultCache.Clear();

        CustomGraphicRaycaster ovrRaycaster = raycast.module as CustomGraphicRaycaster;
        // We're only interested in intersections from OVRRaycasters
        if (ovrRaycaster)
        {
            // The Unity UI system expects event data to have a screen position
            // so even though this raycast came from a world space ray we must get a screen
            // space position for the camera attached to this raycaster for compatability
            leftData.position = ovrRaycaster.GetScreenPosition(raycast);
        }

        // Stick default data values in right and middle slots for compatability

        // copy the apropriate data into right and middle slots
        CustomRayPointerEventData rightData;
        GetPointerData(kMouseRightId, out rightData, true);
        CopyFromTo(leftData, rightData);
        rightData.button = UnityEngine.EventSystems.PointerEventData.InputButton.Right;

        CustomRayPointerEventData middleData;
        GetPointerData(kMouseMiddleId, out middleData, true);
        CopyFromTo(leftData, middleData);
        middleData.button = UnityEngine.EventSystems.PointerEventData.InputButton.Middle;


        m_MouseState.SetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Left, GetPointerButtonFrameState(), leftData);
        m_MouseState.SetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Right, UnityEngine.EventSystems.PointerEventData.FramePressState.NotChanged, rightData);
        m_MouseState.SetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Middle, UnityEngine.EventSystems.PointerEventData.FramePressState.NotChanged, middleData);
        return m_MouseState;
    }

    /// <summary>
    /// Get state for pointer which is a pointer moving in world space across the surface of a world space canvas.
    /// </summary>
    /// <returns></returns>
    protected MouseState GetCanvasPointerData()
    {
        // Get the OVRRayPointerEventData reference
        UnityEngine.EventSystems.PointerEventData leftData;
        GetPointerData(kMouseLeftId, out leftData, true);
        leftData.Reset();

        // Setup default values here. Set position to zero because we don't actually know the pointer
        // positions. Each canvas knows the position of its canvas pointer.
        leftData.position = Vector2.zero;
        leftData.scrollDelta = Input.mouseScrollDelta;
        leftData.button = UnityEngine.EventSystems.PointerEventData.InputButton.Left;

        if (activeGraphicRaycaster)
        {
            // Let the active raycaster find intersections on its canvas
            activeGraphicRaycaster.RaycastPointer(leftData, m_RaycastResultCache);
            RaycastResult raycast = FindFirstRaycast(m_RaycastResultCache);
            leftData.pointerCurrentRaycast = raycast;
            m_RaycastResultCache.Clear();

            CustomGraphicRaycaster ovrRaycaster = raycast.module as CustomGraphicRaycaster;
            if (ovrRaycaster) // raycast may not actually contain a result
            {
                // The Unity UI system expects event data to have a screen position
                // so even though this raycast came from a world space ray we must get a screen
                // space position for the camera attached to this raycaster for compatability
                Vector2 position = ovrRaycaster.GetScreenPosition(raycast);

                leftData.delta = position - leftData.position;
                leftData.position = position;
            }
        }

        // copy the apropriate data into right and middle slots
        UnityEngine.EventSystems.PointerEventData rightData;
        GetPointerData(kMouseRightId, out rightData, true);
        CopyFromTo(leftData, rightData);
        rightData.button = UnityEngine.EventSystems.PointerEventData.InputButton.Right;

        UnityEngine.EventSystems.PointerEventData middleData;
        GetPointerData(kMouseMiddleId, out middleData, true);
        CopyFromTo(leftData, middleData);
        middleData.button = UnityEngine.EventSystems.PointerEventData.InputButton.Middle;

        m_MouseState.SetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Left, StateForMouseButton(0), leftData);
        m_MouseState.SetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Right, StateForMouseButton(1), rightData);
        m_MouseState.SetButtonState(UnityEngine.EventSystems.PointerEventData.InputButton.Middle, StateForMouseButton(2), middleData);
        return m_MouseState;
    }

    /// <summary>
    /// New version of ShouldStartDrag implemented first in PointerInputModule. This version differs in that
    /// for ray based pointers it makes a decision about whether a drag should start based on the angular change
    /// the pointer has made so far, as seen from the camera. This also works when the world space ray is 
    /// translated rather than rotated, since the beginning and end of the movement are considered as angle from
    /// the same point.
    /// </summary>
    private bool ShouldStartDrag(UnityEngine.EventSystems.PointerEventData pointerEvent)
    {
        if (!pointerEvent.useDragThreshold)
            return true;

        if (pointerEvent as CustomRayPointerEventData == null)
        {
            // Same as original behaviour for canvas based pointers
            return (pointerEvent.pressPosition - pointerEvent.position).sqrMagnitude >= eventSystem.pixelDragThreshold * eventSystem.pixelDragThreshold;
        }
        else
        {
            // When it's not a screen space pointer we have to look at the angle it moved rather than the pixels distance
            // For gaze based pointing screen-space distance moved will always be near 0
            Vector3 cameraPos = pointerEvent.pressEventCamera.transform.position;
            Vector3 pressDir = (pointerEvent.pointerPressRaycast.worldPosition - cameraPos).normalized;
            Vector3 currentDir = (pointerEvent.pointerCurrentRaycast.worldPosition - cameraPos).normalized;
            return Vector3.Dot(pressDir, currentDir) < Mathf.Cos(Mathf.Deg2Rad * (angleDragThreshold));
        }
    }

    /// <summary>
    /// Exactly the same as the code from PointerInputModule, except that we call our own
    /// IsPointerMoving.
    /// 
    /// This would also not be necessary if PointerEventData.IsPointerMoving was virtual
    /// </summary>
    /// <param name="pointerEvent"></param>
    protected override void ProcessDrag(UnityEngine.EventSystems.PointerEventData pointerEvent)
    {
        //we'll assume we determined pointer is moving already
        //bool moving = IsPointerMoving(pointerEvent);
        if (pointerEvent.pointerDrag != null
            && !pointerEvent.dragging
            && ShouldStartDrag(pointerEvent))
        {
            UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.beginDragHandler);
            pointerEvent.dragging = true;
        }

        // Drag notification
        if (pointerEvent.dragging && pointerEvent.pointerDrag != null)
        {
            // Before doing drag we should cancel any pointer down state
            // And clear selection!
            if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
            {
                UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;
            }
            UnityEngine.EventSystems.ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, UnityEngine.EventSystems.ExecuteEvents.dragHandler);
        }
    }

    /// <summary>
    /// Get pressed/released state of conditions and return corresponding pointerevent data framePressState
    /// formerly: GetGazeButtonState()
    /// </summary>
    /// <returns></returns>
    protected UnityEngine.EventSystems.PointerEventData.FramePressState GetPointerButtonFrameState()
    {
        //This is for android runtime
#if UNITY_ANDROID && !UNITY_EDITOR
            pressed |= Input.GetMouseButtonDown(0);
            released |= Input.GetMouseButtonUp(0);
#endif

        if (m_inputModuleSettings.PressedCondition && m_inputModuleSettings.ReleasedCondition)
        {
            return UnityEngine.EventSystems.PointerEventData.FramePressState.PressedAndReleased;
        }
        if (m_inputModuleSettings.PressedCondition)
        {
            return UnityEngine.EventSystems.PointerEventData.FramePressState.Pressed;
        }
        if (m_inputModuleSettings.ReleasedCondition)
        {
            return UnityEngine.EventSystems.PointerEventData.FramePressState.Released;
        }

        return UnityEngine.EventSystems.PointerEventData.FramePressState.NotChanged;
    }
    -------------------END ORIGINAL-----------*/
}