using UnityEngine;

public class M_TriggerPull : MonoBehaviour
{
    [SerializeField] TriggerPull m_triggerPull = new TriggerPull();

    private void Awake()
    {
        m_triggerPull.Start();
    }
    private void Update()
    {
        m_triggerPull.Update(Time.deltaTime);
    }

    public void TriggerDown() =>
        m_triggerPull.TriggerDown();

    public void TriggerHold() =>
        m_triggerPull.TriggerHold();

    public void TriggerRelease() =>
        m_triggerPull.TriggerRelease();
}
