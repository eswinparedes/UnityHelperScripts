using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BasicVRHand : MonoBehaviour
{
    [SerializeField] BasicVRHand m_hand = default;

    private void Start()
    {
        m_hand.Start();
    }

    private void Update()
    {
        m_hand.UpdateAnimStates();
    }
}
