using UnityEngine;
using System;

public class CustomDebug : MonoBehaviour
{
    [SerializeField] bool m_doesLogStates = false;
    [SerializeField] bool m_doesLogEvents = false;
    [SerializeField] bool m_doesLogMisc = false;
    [SerializeField] bool m_doesLogSaveLoad = false;

    static Action<Action> template = (x) => x();
    static Action<Action> doNothing = (x) => { };

    public static Action<Action> DebugLogState = doNothing;
    public static Action<Action> DebugLogEvent = doNothing;
    public static Action<Action> DebugLogMisc = doNothing;
    public static Action<Action> DebugLogSaveLoad = doNothing;

    public static Action<Action> DebugLogPermanent = template;

    private void Awake()
    {
        DebugLogEvent = m_doesLogEvents ? template : doNothing;
        DebugLogState = m_doesLogStates ? template : doNothing;
        DebugLogMisc = m_doesLogMisc ? template : doNothing;
        DebugLogSaveLoad = m_doesLogSaveLoad ? template : doNothing;
    }
}
