using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_Tag : SO_A_StringReadOnly
{
    [SerializeField] string m_tag = default;

    public override string Value { get => m_tag; set  { } }
}
