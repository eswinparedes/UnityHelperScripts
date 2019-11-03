using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BoolTrifectaTest : MonoBehaviour
{
    BoolTrifecta State = new BoolTrifecta();

    private void Update()
    {
        State = State.GetUpdateFromInput(Input.GetKey(KeyCode.Space));
        Debug.Log(State);
    }
}
