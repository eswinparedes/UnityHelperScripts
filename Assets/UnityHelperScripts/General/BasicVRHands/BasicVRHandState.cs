using UnityEngine;

[System.Serializable]
public struct BasicVRHandState 
{
    [Header("Gestures - Top Gestures Take Priority")]
    [SerializeField] float m_flex;
    [SerializeField] float m_pinch;
    [Header("Masked Gestures")]
    [SerializeField] float m_thumbsUp;
    [SerializeField] float m_point;
    [Header("Hand Pose Current")]
    [SerializeField] HandPoseId m_pose;

    public float Flex => m_flex;
    public float Pinch => m_pinch;
    public float ThumbsUp => m_thumbsUp;
    public float Point => m_point;
    public HandPoseId Pose => m_pose;

    public BasicVRHandState(float flex, float pinch, float thumbsUp, float point, HandPoseId pose)
    {
        this.m_flex = flex;
        this.m_pinch = pinch;
        this.m_thumbsUp = thumbsUp;
        this.m_point = point;
        this.m_pose = pose;
    }
}

public static class BasicVRHandsExtensions
{
    public static BasicVRHandState With
        (this BasicVRHandState @this, float? flex = null, float? pinch = null, float? thumbsUp = null, float? point = null, HandPoseId? pose = null) =>
        new BasicVRHandState(
            flex ?? @this.Flex,
            pinch ?? @this.Pinch,
            thumbsUp ?? @this.ThumbsUp,
            point ?? @this.Point,
            pose ?? @this.Pose);
}
