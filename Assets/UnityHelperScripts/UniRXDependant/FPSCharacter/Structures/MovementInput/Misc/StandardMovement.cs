using SUHScripts.Functional;
using UnityEngine;
using static SUHScripts.Functional.Functional;

[System.Serializable]
public class StandardMovement
{
    [SerializeField] float m_baseSpeed;
    [SerializeField] float m_boostSpeed;
    [SerializeField] float m_acceleration;

    public float BaseSpeed { get => m_baseSpeed; }
    public float BoostSpeed { get => m_boostSpeed;}
    public Option<float> Acceleration => m_acceleration < 0 ?  NONE : m_acceleration.AsOption();

    public StandardMovement(float baseSpeed, float boostSpeed, float acceleration)
    {
        this.m_baseSpeed = baseSpeed;
        this.m_boostSpeed = boostSpeed;
        this.m_acceleration = acceleration;
    }
}
