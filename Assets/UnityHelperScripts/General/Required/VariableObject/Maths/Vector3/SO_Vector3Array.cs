using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Vector3/Vector3 Array")]
public class SO_Vector3Array : ScriptableObject
{
    public Vector3[] vectors = default;
}
[System.Serializable]
public class CL_Vector3ArrayReference
{
    [SerializeField] SO_Vector3Array vectors = default;

    public Vector3[] Vectors
    {
        get { return vectors.vectors; }
    }
}
