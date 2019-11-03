using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class M_StandardFPSStateMachine : MonoBehaviour
{
    [SerializeField] GroundedMovement m_groundedMovement = default;

    private void Start()
    {
        m_groundedMovement.Subscribe();
    }
}
