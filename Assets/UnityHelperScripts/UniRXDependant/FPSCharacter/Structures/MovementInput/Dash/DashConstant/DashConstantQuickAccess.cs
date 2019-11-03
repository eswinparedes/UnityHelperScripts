using UnityEngine;

[System.Serializable]
public class DashConstantQuickAccess : DashConstant
{
    public float m_boostSpeed = 30;
    public float m_cooldownLength;
    public float m_dashTimeLength = .25f;
    public AnimationCurve m_dashCurve = default;

    public override float BoostSpeed => m_boostSpeed;
    public override float CoolDownLength => m_cooldownLength;
    public override float DashTimeLength => m_dashTimeLength;
    public override AnimationCurve DashCurve => m_dashCurve;

}