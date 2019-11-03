using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Component/Component Manager")]
public class SO_ComponentManager : ScriptableObject
{
    List<I_QuickComponent> m_components = new List<I_QuickComponent>();

    public float DeltaTime { get { return m_deltaTime; } }

    float m_deltaTime;

    public void Reset()
    {
        m_components.Clear();
    }

    public void Execute(float deltaTime)
    {
        m_deltaTime = deltaTime;

        for(int i = 0; i < m_components.Count; i++)
        {
            m_components[i].Execute();
        }
    }
    public void AddComponent(I_QuickComponent component)
    {
        if (!m_components.Contains(component))
        {
            m_components.Add(component);
        }
    }
    public void RemoveComponent(I_QuickComponent component)
    {
        if (m_components.Contains(component))
        {
            m_components.Remove(component);
        }
    }

    public bool ContainsCompoenent(I_QuickComponent component)
    {
        return m_components.Contains(component);
    }


}


