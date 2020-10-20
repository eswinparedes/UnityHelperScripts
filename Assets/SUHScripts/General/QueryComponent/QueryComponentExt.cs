using SUHScripts.Functional;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public static class QueryComponentExt 
    {
        public static void ModifyToRouteComponentQueriesTo(GameObject routTo, params Collider[] colliders)
        {
            if (colliders.Length == 0)
            {
                Debug.LogError("'colliders' array has no values, was this on purpose?");
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                var source = colliders[i].gameObject.GetOrAddComponent<PushableQuerySource>();
                source.PushSource(routTo);
            }
        }

        class PushableQuerySource : A_QueryComponentSource
        {
            GameObject m_source;

            public void PushSource(GameObject source)
            {
                m_source = source;
            }
            public override Option<T> QueryComponentOption<T>() =>
                m_source.GetComponentOption<T>();
        }
    }
}
