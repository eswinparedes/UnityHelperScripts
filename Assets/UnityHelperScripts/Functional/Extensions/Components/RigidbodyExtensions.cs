using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RigidbodyExtensions 
{
    public static void StopAllMomentum(this Rigidbody body)
    {
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }

    public static void StopAllMomentum(this Rigidbody2D @this)
    {
        @this.angularVelocity = 0;
        @this.velocity = Vector2.zero;
    }
}
