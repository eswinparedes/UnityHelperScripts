using SUHScripts.Functional;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SUHScripts.Functional.Functional;

public class M_GameObjectListQueryComponents : A_QueryComponentSource
{
    [SerializeField] List<GameObject> m_componentCastingSources = default;

    public override Option<T> QueryComponentOption<T>()
    {
        for(int i = 0; i < m_componentCastingSources.Count; i++)
        {
            var opt = m_componentCastingSources[i].GetComponentOption<T>();
            if (opt.IsSome)
                return opt.Value;
        }

        return NONE;
    }
}
