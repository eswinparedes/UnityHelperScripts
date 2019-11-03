using SUHScripts.Functional;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_QueryComponentSource : MonoBehaviour
{
    public abstract Option<T> QueryComponentOption<T>();
}
