using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "SUHS/FPS/Movement/Character Walk Run Jump")]
public class CHAR_WalkRunJump : CHAR_MovementSource
{
    [Header("Standard Movement Stats")]
    public StandardMovement m_movementSettings = new StandardMovement(5, 10, -1);
    public DashInstantQuickAccess m_jumpSettings = default;

    public StandardMovement MovementSettings => m_movementSettings;
    public DashInstant JumpSettings => m_jumpSettings;

    public override CHAR_MovementInstance Build(FPSRoot root) =>
        new CHAR_WRJInstance(root, this);
}