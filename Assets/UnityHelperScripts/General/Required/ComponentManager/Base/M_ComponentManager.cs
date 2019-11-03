using UnityEngine;
using System;

public class M_ComponentManager : MonoBehaviour {

    //TODO: RUN BACKWARDS
    [SerializeField] SO_A_FloatReadWrite m_time_deltaTime = default;
    [SerializeField] SO_A_FloatReadWrite m_time_fixedDeltaTime = default;
    [SerializeField] SO_ComponentManager[] componentManagers_UPDATE = default; 
    [SerializeField] SO_ComponentManager[] componentManagers_LATE_UPDATE = default;
    [SerializeField] SO_ComponentManager[] componentManagers_FIXED_UPDATE = default;

    private void OnDisable()
    {
        for (int i = 0; i < componentManagers_UPDATE.Length; i++)
        {
            componentManagers_UPDATE[i].Reset();
        }

        for (int i = 0; i < componentManagers_FIXED_UPDATE.Length; i++)
        {
            componentManagers_FIXED_UPDATE[i].Reset();
        }

        for (int i = 0; i < componentManagers_LATE_UPDATE.Length; i++)
        {
            componentManagers_LATE_UPDATE[i].Reset();
        }
    }
    private void Update()
    {
        m_time_deltaTime.Value = Time.deltaTime;
        for (int i = 0; i < componentManagers_UPDATE.Length; i++)
        {
            componentManagers_UPDATE[i].Execute(Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < componentManagers_LATE_UPDATE.Length; i++)
        {
            componentManagers_LATE_UPDATE[i].Execute(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        m_time_fixedDeltaTime.Value = Time.fixedDeltaTime;
        for (int i = 0; i < componentManagers_FIXED_UPDATE.Length; i++)
        {
            componentManagers_FIXED_UPDATE[i].Execute(Time.fixedDeltaTime);
        }
    } 
}