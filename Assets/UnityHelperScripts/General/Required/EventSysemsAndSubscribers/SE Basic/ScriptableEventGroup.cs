using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ScriptableEventGroup
{
    [Space(10)]
    [SerializeField] SO_A_ScriptableEvent m_eventToSubscribe = default;
    [SerializeField] UnityEvent m_onEventRaised = new UnityEvent();

    public void Subscribe() =>
        m_eventToSubscribe.OnEventRaised += m_onEventRaised.Invoke;

    public void Unsubscribe() =>
        m_eventToSubscribe.OnEventRaised -= m_onEventRaised.Invoke;
}
