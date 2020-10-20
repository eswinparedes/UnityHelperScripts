using SUHScripts.Functional;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public abstract class A_QueryComponentSource : MonoBehaviour
    {
        public abstract Option<T> QueryComponentOption<T>();
    }

}
