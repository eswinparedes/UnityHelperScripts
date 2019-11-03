using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Other/Animation Curve/Animation Curve")]
public class SO_AnimationCurve: SO_A_AnimationCurveReadWrite
{
    [SerializeField] AnimationCurve m_value = new AnimationCurve();

    public override AnimationCurve Value
    {
        get => m_value;
        set => m_value = value;
    }
}