using SUHScripts.Functional;
using UnityEngine;

public abstract class DashConstant
{
    public abstract float BoostSpeed { get; }
    public abstract float CoolDownLength { get; }
    public abstract float DashTimeLength { get; }
    public abstract AnimationCurve DashCurve { get; }

}