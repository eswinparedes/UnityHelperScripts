using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicREMovement : MonoBehaviour
{
    [SerializeField] float m_moveSpeed = 3f;
    [SerializeField] float m_turnSpeed = 10f;
    private void Update()
    {
        var turn = Input.GetAxis("Horizontal");
        var forward = Input.GetAxis("Vertical");

        this.transform.position += this.transform.forward * m_moveSpeed * Time.deltaTime * forward;
        this.transform.Rotate(Vector3.up * m_turnSpeed * Time.deltaTime * turn);
    }
}
