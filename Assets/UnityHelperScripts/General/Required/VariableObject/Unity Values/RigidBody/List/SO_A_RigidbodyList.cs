using System.Collections.Generic;
using UnityEngine;

public abstract class SO_A_RigidbodyList : ScriptableObject
{
    public abstract List<Rigidbody> Rigidbodies { get; set; }
    public abstract void AddRigidbody(Rigidbody rb);
    public abstract void RemoveRigidbody(Rigidbody rb);
}
