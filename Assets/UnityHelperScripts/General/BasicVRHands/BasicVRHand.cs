using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BasicVRHand
{
    [SerializeField] BasicVRHandState m_currentState = default;
    [SerializeField] private Animator m_animator = null;
    [SerializeField] Transform m_root = default;
    [SerializeField] UnityEvent m_onActivateHandsEvent = new UnityEvent();
    [SerializeField] UnityEvent m_onDeactivateHandsEvent = new UnityEvent();

    public Transform Root => m_root;
    public UnityEvent OnActivateHandsEvent => m_onActivateHandsEvent;
    public UnityEvent OnDeactivateHandsEvent => m_onDeactivateHandsEvent;
    public BasicVRHandState CurrentState => m_currentState;

    public const string ANIM_LAYER_NAME_POINT = "Point Layer";
    public const string ANIM_LAYER_NAME_THUMB = "Thumb Layer";
    public const string ANIM_PARAM_NAME_FLEX = "Flex";
    public const string ANIM_PARAM_NAME_POSE = "Pose";

    private int m_animLayerIndexThumb = -1;
    private int m_animLayerIndexPoint = -1;
    private int m_animParamIndexFlex = -1;
    private int m_animParamIndexPose = -1;

    public void Start()
    {
        m_animLayerIndexPoint = m_animator.GetLayerIndex(ANIM_LAYER_NAME_POINT);
        m_animLayerIndexThumb = m_animator.GetLayerIndex(ANIM_LAYER_NAME_THUMB);
        m_animParamIndexFlex = Animator.StringToHash(ANIM_PARAM_NAME_FLEX);
        m_animParamIndexPose = Animator.StringToHash(ANIM_PARAM_NAME_POSE);
    }

    public void UpdateAnimStates()
    {
        m_animator.SetInteger(m_animParamIndexPose, (int) m_currentState.Pose);
        m_animator.SetFloat(m_animParamIndexFlex, m_currentState.Flex);
        m_animator.SetLayerWeight(m_animLayerIndexPoint, m_currentState.Point);
        m_animator.SetLayerWeight(m_animLayerIndexThumb, m_currentState.ThumbsUp);

        m_animator.SetFloat("Pinch", m_currentState.Pinch);
    }

    public void ApplyBasicVRHandState(BasicVRHandState state)
    {
        m_currentState = state;
    }

    public void ActivateHands()
    {
        m_onActivateHandsEvent.Invoke();
    }

    public void DeactivateHands()
    {
        m_onDeactivateHandsEvent.Invoke();
    }
}
