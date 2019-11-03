using SUHScripts.Functional;
using System;
using UnityEngine;
using static SUHScripts.Functional.Functional;

public class DashConstantFunctionSource : DashConstant
{
    public override float BoostSpeed => m_boostSpeed();
    public override float CoolDownLength => m_cooldownLength();
    public override float DashTimeLength => m_timeSource();
    public override AnimationCurve DashCurve => m_animCurveSource();

    private Func<float> m_boostSpeed;
    private Func<float> m_cooldownLength;
    private Func<float> m_timeSource;
    private Func<AnimationCurve> m_animCurveSource;

    public DashConstantFunctionSource(
        Func<float> boostSpeed, Func<float> cooldownLength,
        Func<AnimationCurve> animCurveSource, Func<float> timeSource)
    {
        this.m_boostSpeed = boostSpeed;
        this.m_cooldownLength = cooldownLength;
        this.m_timeSource = timeSource;
        this.m_animCurveSource = animCurveSource;
    }

}