using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class M_EventSystemPointerReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] UnityEvent m_onPointerEnter = new UnityEvent();
    [SerializeField] UnityEvent m_onPointerExit = new UnityEvent();
    [SerializeField] UnityEvent m_onPointerDown = new UnityEvent();
    [SerializeField] UnityEvent m_onPointerUp = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        m_onPointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_onPointerUp.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_onPointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_onPointerExit.Invoke();
    }
}
