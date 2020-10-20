using UnityEngine;
using UnityEngine.Events;

namespace SUHScripts.Tests
{
    public class M_FocusableEvents : MonoBehaviour, I_Focusable
    {
        [SerializeField] UnityEvent m_onFocusStartEvent = new UnityEvent();
        [SerializeField] UnityEvent m_onFocusEndEvent = new UnityEvent();

        public void OnFocusStart()
        {
            Debug.Log("Focus start");
            m_onFocusStartEvent.Invoke();
        }

        public void OnFocusEnd()
        {
            Debug.Log("Focus end");
            m_onFocusEndEvent.Invoke();
        }

    
    }
}

