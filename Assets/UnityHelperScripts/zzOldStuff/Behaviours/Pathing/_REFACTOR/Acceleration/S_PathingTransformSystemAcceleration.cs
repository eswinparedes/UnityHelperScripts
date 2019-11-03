using UnityEngine;
using System;

public static class S_PathingTransformSystemAcceleration
{
    public static void Initialize(this I_PathingAcceleration deps)
    {
        S_PathingTransformSystem.InitializeBasicPathing(deps);

        deps.PathingBehaviour = deps.UseRigidBody ? (Action<I_PathingAcceleration, float>)MoveRigidBody : MoveTransform;

        deps.PathingCompleteBehaviour = (d) =>
        {
            S_PathingTransformSystem.PathingComplete(d);
        };
        deps.WaitTimer.OnTimerComplete = () =>
        {
            S_PathingTransformSystem.WaitComplete(deps);
            deps.ActiveBehaviour = (x) =>
            {
                deps.PathingBehaviour(deps, x);
            };
        };
    }

    static void MoveRigidBody(I_PathingAcceleration deps, float deltaTime)
    {
        Vector3 smooth = GetSmoothPosition(deps, deltaTime);
        deps.PathingRigidBody.MovePosition(smooth);
        CheckForCompletion(deps);
    }

    static void MoveTransform(I_PathingAcceleration deps, float deltaTime)
    {
        Vector3 smooth = GetSmoothPosition(deps, deltaTime);
        deps.PathingTransform.position = smooth;
        CheckForCompletion(deps);
    }

    static Vector3 GetSmoothPosition(I_PathingAcceleration deps, float deltaTime)
    {
        Vector3 vel = deps.VelocityReference;

        Vector3 smooth = Vector3.SmoothDamp(
            deps.PathingTransform.position,
            deps.GoalPathPoint.PathPointPosition + deps.Offset,
            ref vel,
            deps.SmoothTime,
            deps.PathingSpeed);
        deps.VelocityReference = vel;

        return smooth;
    }

    static void CheckForCompletion(I_PathingAcceleration deps)
    {
        if (Vector3.Distance(deps.PathingTransform.position, deps.GoalPathPoint.PathPointPosition + deps.Offset) <= deps.MinDistance)
        {
            deps.PathingCompleteBehaviour(deps);
        }
    }

    

}
