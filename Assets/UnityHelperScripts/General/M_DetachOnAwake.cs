using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DetachOnAwake : MonoBehaviour
{
    private void Awake()
    {
        this.transform.parent = null;
    }
}
