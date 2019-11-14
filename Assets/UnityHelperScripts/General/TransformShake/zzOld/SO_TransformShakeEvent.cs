using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Experimental/Transform Shake Event")]
[System.Obsolete]
public class SO_TransformShakeEvent : ScriptableObject
{
    List<I_TransformShakeSubscriber> m_transformShakes = new List<I_TransformShakeSubscriber>();

    public void AddShakeEvent(SO_A_NoiseData data)
    {       
        for(int i = 0; i < m_transformShakes.Count; i++)
        {
            m_transformShakes[i].AddShakeEvent(data);
        }
    }

    public void AddShakeEvent(SO_TransfromShakeData data)
    {
        for(int i  = 0; i < m_transformShakes.Count; i++)
        {
            m_transformShakes[i].AddShakeEvent(data);
        }
    }

    public void AddTransformShake(I_TransformShakeSubscriber item)
    {
        if (!m_transformShakes.Contains(item))
        {
            m_transformShakes.Add(item);
        }
    }

    public void RemoveTransformShake(I_TransformShakeSubscriber item)
    {
        if (m_transformShakes.Contains(item))
        {
            m_transformShakes.Remove(item);
        }
    }

    private void OnEnable()
    {
        m_transformShakes.Clear();
    }

    private void OnDisable()
    {
        m_transformShakes.Clear();
    }
}
