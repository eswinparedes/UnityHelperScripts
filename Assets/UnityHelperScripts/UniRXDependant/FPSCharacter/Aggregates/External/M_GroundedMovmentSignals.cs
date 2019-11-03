using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class M_GroundedMovmentSignals : MonoBehaviour
{
    [SerializeField] FPSRoot m_root = default;
    [SerializeField] FPSBasicGroundedSignals m_basicSignals = default;
    
    private void Start()
    {
        m_basicSignals.Initialize(m_root.AttachBehaviour, m_root);
        m_basicSignals.Subscribe();
    }
}
