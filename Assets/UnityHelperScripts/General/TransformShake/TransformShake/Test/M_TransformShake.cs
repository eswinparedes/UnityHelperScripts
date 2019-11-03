using System.Collections.Generic;
using UnityEngine;

public class M_TransformShake : A_Component
{
    [SerializeField] TransformShakeBehaviour m_transformShakeBehaviour = default;

    public override void Execute()
    {
        m_transformShakeBehaviour.Execute(m_componentManager.DeltaTime);
    }

    private void Start()
    {
        m_transformShakeBehaviour.Start();
    }
}