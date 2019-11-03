using UnityEngine;
using UnityEngine.EventSystems;
using static MathHelpers.MathHelper;

public class M_WorldPointCustomInputModuleData : A_Component
{
    [SerializeField] Camera WorldCamera = default;
    [SerializeField] Transform m_worldPoint = default;
    [SerializeField] SO_CustomInputModuleData m_data = default;
    [SerializeField] UnityEvent_Float m_pressAlphaUpdate = default;

    [Header("Other Inputs")]
    [SerializeField] SO_A_Bool m_submitCondition = default;
    [SerializeField] SO_A_Bool m_cancelCondition = default;
    [Header("Input Axes")]
    [SerializeField] SO_A_Float m_horizontalAxis = default;
    [SerializeField] SO_A_Float m_verticalAxis = default;
    [SerializeField] SO_A_Bool m_horizontalAxisThisFrame = default;
    [SerializeField] SO_A_Bool m_verticalAxisThisFrame = default;
    [Header("Other")]
    [SerializeField] float m_pressThreshold = default;
    [SerializeField] float m_hoverThreshold = default;

    GameObject m_lastGameObject;
    BoolTrifecta m_lastState = default;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        m_data.GetProcessedResult += GetProcessedResult;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_data.GetProcessedResult -= GetProcessedResult;
    }
    public Ray RaycastRay
    {
        get
        {
            Vector3 transformOnScreenPosition = WorldCamera.WorldToScreenPoint(m_worldPoint.position);
            Vector3 screenPositionOnWorld = WorldCamera.ScreenToWorldPoint(transformOnScreenPosition);

            Vector3 dir = (m_worldPoint.position - screenPositionOnWorld).normalized;

            Ray ray = WorldCamera.ScreenPointToRay(transformOnScreenPosition);
            ray = new Ray(m_worldPoint.position, ray.direction);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);

            return ray;
        }
    }

    public override void Execute()
    {
        m_data.InputRay = RaycastRay;
    }

    public RaycastResult GetProcessedResult(RaycastResult result)
    {
        bool resultIsValid = result.isValid;

        float distance = Vector3.Distance(result.worldPosition, m_data.InputRay.origin);
        bool resultIsInPressRange = IsGameObjectInRange(result, distance, m_pressThreshold);
        bool resultIsInHoverRange = IsGameObjectInRange(result, distance, m_hoverThreshold);
        bool allowResult = resultIsValid && (resultIsInPressRange || resultIsInHoverRange);

        GameObject resultObject = allowResult ? result.gameObject : null;

        bool resultIsSameObjectAsLast = result.gameObject == m_lastGameObject;
        bool input = resultIsValid && resultIsInPressRange && resultIsSameObjectAsLast;

        EventSystem.current.SetSelectedGameObject(resultIsInPressRange ? resultObject : null);
        result.gameObject = resultObject;
        m_lastGameObject = resultObject;
        Cursor.lockState = CursorLockMode.None;

        m_lastState = m_lastState.GetUpdateFromInput(input);

        (_, m_data.PressedCondition, _, m_data.ReleasedCondition) = m_lastState.Deconstruct();

        float remap = Remap(distance, new Vector2(m_pressThreshold * 3, m_pressThreshold), Range01);

        return result;
    }



    public bool IsGameObjectInRange(RaycastResult result, float distance, float range) =>
        result.isValid ?
        distance <= range :
        false;

   
    public void GetRidOfShittyWarnings()
    {
        var a = m_horizontalAxis;
        var b = m_verticalAxis;
        var c = m_horizontalAxisThisFrame;
        var d = m_verticalAxisThisFrame;
        var e = m_cancelCondition;
        var f = m_pressAlphaUpdate;
        var g = m_submitCondition;

    }
}

/*
public class SO_WorldPointInputModuleData : A_CustomInputModuleSettings
{
    [Header("Left Click")]
    [SerializeField] SO_A_BoolReadWrite m_pressedCondition;
    [SerializeField] SO_A_BoolReadWrite m_releasedCondition;
    [Header("Other Inputs")]
    [SerializeField] SO_A_Bool m_submitCondition;
    [SerializeField] SO_A_Bool m_cancelCondition;
    [Header("Input Axes")]
    [SerializeField] SO_A_Float m_horizontalAxis;
    [SerializeField] SO_A_Float m_verticalAxis;
    [SerializeField] SO_A_Bool m_horizontalAxisThisFrame;
    [SerializeField] SO_A_Bool m_verticalAxisThisFrame;
    [Header("Events")]
    [SerializeField] SO_A_GameEvent m_onInputModuleProcessComplete;
    [SerializeField] SO_A_GameEvent m_onInputModulePointerDown;
    [SerializeField] SO_A_GameEvent m_onInputModulePointerUp;
    [Header("Other")]
    [SerializeField] float m_pressThreshold;
    [SerializeField] float m_hoverThreshold;
    [SerializeField] SO_A_Float m_pressAlpha;

    public float HoverThreshold => m_hoverThreshold;

    GameObject m_lastGameObject;
    TripleToggler m_lastState = new TripleToggler(false, false, false, false);

    #region abstract members
    public override bool PressedCondition { get => m_pressedCondition.IsTrue; set => m_pressedCondition.IsTrue = value; }
    public override bool ReleasedCondition { get => m_releasedCondition.IsTrue; set => m_releasedCondition.IsTrue = value; }

    public override bool SubmitCondition { get => m_submitCondition.IsTrue; set => m_submitCondition.IsTrue = value; }
    public override bool CancelCondition { get => m_cancelCondition.IsTrue; set => m_cancelCondition.IsTrue = value; }

    public override float HorizontalAxis { get => m_horizontalAxis.Value; set => m_horizontalAxis.Value = value; }
    public override float VerticalAxis { get => m_horizontalAxis.Value; set => m_horizontalAxis.Value = value; }
    public override bool HorizontalAxisThisFrame { get => m_horizontalAxisThisFrame.IsTrue; set => m_horizontalAxisThisFrame.IsTrue = value; }
    public override bool VerticalAxisThisFrame { get => m_horizontalAxisThisFrame.IsTrue; set => m_horizontalAxisThisFrame.IsTrue = value; }

    public override Ray InputRay { get; set; }
    public override List<RaycastResult> InputRaycastResult { get; set; }

    public float PressAlpha => m_pressAlpha.Value;

    public override RaycastResult GetProcessedResult(RaycastResult result)
    {
        bool resultIsValid = result.isValid;

        float distance = Vector3.Distance(result.worldPosition, InputRay.origin);
        bool resultIsInPressRange = IsGameObjectInRange(result, distance, m_pressThreshold);
        bool resultIsInHoverRange = IsGameObjectInRange(result, distance, m_hoverThreshold);
        bool allowResult = resultIsValid && (resultIsInPressRange || resultIsInHoverRange);

        GameObject resultObject = allowResult ? result.gameObject : null;

        bool resultIsSameObjectAsLast = result.gameObject == m_lastGameObject;
        bool input = resultIsValid && resultIsInPressRange && resultIsSameObjectAsLast;

        EventSystem.current.SetSelectedGameObject(resultIsInPressRange ? resultObject : null);
        result.gameObject = resultObject;
        m_lastGameObject = resultObject;
        Cursor.lockState = CursorLockMode.None;

        m_lastState = ProcessTrippleToggler(input, m_lastState);
        (m_pressedCondition.IsTrue, _, m_releasedCondition.IsTrue) = m_lastState.DeconstructFrames;

        float remap = Remap(distance, new Vector2(m_pressThreshold * 3, m_pressThreshold), Range01);
  
        m_pressAlpha.Value = distance <= m_pressThreshold * 3 ?
            remap < 0 ? 1 : remap
            : 0f;

        return result;
    }

    #endregion

    #region events
    public override void OnInputModulePointerDown() => m_onInputModulePointerDown.Raise();

    public override void OnInputModulePointerUp() => m_onInputModulePointerUp.Raise();

    public override void OnInputModuleProcessComplete() => m_onInputModuleProcessComplete.Raise();
    #endregion

    #region HelperFunctions
    public bool IsGameObjectInRange(RaycastResult result, float distance, float range) =>
        result.isValid ?
        distance <= range :
        false;
    #endregion
}
*/
