using UnityEngine;

public class M_CycleUnityEvent : MonoBehaviour
{
    [SerializeField] EventCycler m_eventCycles = default;

    private void Start()
    {
        m_eventCycles.Start();
    }

    public void CycleNext() => m_eventCycles.CycleNext();
}