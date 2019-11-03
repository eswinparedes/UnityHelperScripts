using UnityEngine;
using System;

public static class S_PathingTransformSystemLerp
{
    public static void InitializeLerp(this I_PathingLerp deps)
    {
        S_PathingTransformSystem.InitializeBasicPathing(deps);

        deps.PathingBehaviour = deps.UseRigidBody ? (Action<I_PathingLerp, float>)UpdatePathingLerpRigidBody : UpdatePathingLerpTransform;

        deps.PreviousPathPoint = deps.GoalPathPoint;
        S_PathingTransformSystem.SetNextPathPoint(deps);

        deps.LerpTimer = new C_Timer();
        deps.LerpTimer.SetTimerLength(deps.PathingSpeed);
        deps.LerpTimer.OnTimerComplete = () =>
        {
            S_PathingTransformSystem.PathingComplete(deps);
            deps.LerpTimer.Restart();
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

    public static void UpdatePathingLerpTransform(I_PathingLerp deps, float deltaTime)
    {
        deps.LerpTimer.UpdateTimer(deltaTime);

        float curve = deps.LerpCurve.Evaluate(deps.LerpTimer.Alpha);
        deps.PathingTransform.position =
            Vector3.Lerp(deps.PreviousPathPoint.PathPointPosition + deps.Offset, deps.GoalPathPoint.PathPointPosition + deps.Offset, curve);
    }

    public static void UpdatePathingLerpRigidBody(I_PathingLerp deps, float deltaTime)
    {
        deps.LerpTimer.UpdateTimer(deltaTime);

        float curve = deps.LerpCurve.Evaluate(deps.LerpTimer.Alpha);
        deps.PathingRigidBody.MovePosition(Vector3.Lerp(deps.PreviousPathPoint.PathPointPosition, deps.GoalPathPoint.PathPointPosition, curve));
    }
}
