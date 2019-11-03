using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SUHS/FPS/Movement/Character Dash")]
public class CHAR_Dash : CHAR_MovementSource
{
    [SerializeField] DashConstantQuickAccess m_dashSettings = default;
    [SerializeField] bool m_doesDashToView = false;

    public DashConstant DashSettings =>
        m_dashSettings;

    public bool DoesDashToView =>
        m_doesDashToView;

    public override CHAR_MovementInstance Build(FPSRoot root) =>
        new CHAR_DashInstance(root, this);
}
