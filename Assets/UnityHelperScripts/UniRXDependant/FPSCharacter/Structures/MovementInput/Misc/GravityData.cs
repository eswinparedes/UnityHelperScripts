using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GravityData
{
    [SerializeField] float m_gravity = -9.8f;
    [SerializeField] float m_terminalVelocity = -30;
    
    public float Gravity => m_gravity;
    public float TerminalVelocity => m_terminalVelocity;

    public GravityData(float gravity, float terminalVel = -20 )
    {
        this.m_gravity = gravity;
        this.m_terminalVelocity = terminalVel;
    }
}
