using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PrefabPoolingSystemBootsrap : MonoBehaviour
{
    private void OnEnable()
    {
        PrefabPoolingSystem.Reset();
    }

    private void OnDisable()
    {
        PrefabPoolingSystem.Reset();
    }
}
