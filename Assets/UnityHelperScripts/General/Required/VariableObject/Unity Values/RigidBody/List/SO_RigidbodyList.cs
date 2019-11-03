using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/Rigidbody/Rigidbody List")]
public class SO_RigidbodyList : SO_A_RigidbodyList
{
    List<Rigidbody> m_rigidBodies = new List<Rigidbody>();
    
    public override List<Rigidbody> Rigidbodies { get => m_rigidBodies; set { } }

    public override void AddRigidbody(Rigidbody rb)
    {
        if (!m_rigidBodies.Contains(rb))
        {
            m_rigidBodies.Add(rb);
        }
    }

    public override void RemoveRigidbody(Rigidbody rb)
    {
        if (m_rigidBodies.Contains(rb))
        {
            m_rigidBodies.Remove(rb);
        }
    }
}
