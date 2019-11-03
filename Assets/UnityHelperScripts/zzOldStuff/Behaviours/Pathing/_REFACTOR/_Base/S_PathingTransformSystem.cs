using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class S_PathingTransformSystem
{
    public static void InitializeBasicPathing(I_PathingBasic deps)
    {
        deps.GoalPathPointIndex = deps.StartIndex;
        deps.GoalPathPoint = deps.PathPoints[deps.GoalPathPointIndex];

        deps.Offset = -deps.PathingTransform.GetOffsetTo(deps.GoalPathPoint.PathPointPosition, deps.OffsetMask);

        deps.WaitTimer = new C_Timer();
        deps.WaitTimer.SetTimerLength(deps.StartDelay);
        deps.WaitTimer.OnTimerComplete = () => WaitComplete(deps);

        deps.ActiveBehaviour = (x) =>
        {
            UpdateWaiting(deps, x);
        };
    }

    public static void PathingComplete(I_PathingBasic deps)
    {
        deps.ActiveBehaviour = (x) =>
        {
            UpdateWaiting(deps, x);
        };

        deps.PreviousPathPoint = deps.GoalPathPoint;
        SetNextPathPoint(deps);
    }

    public static void WaitComplete(I_PathingBasic deps)
    {
        deps.WaitTimer.SetTimerLength(deps.PathPointDelay);
    }

    public static void UpdateWaiting(I_PathingBasic deps, float deltaTime)
    {
        deps.WaitTimer.UpdateTimer(deltaTime);
    }

    public static void SetNextPathPoint(I_PathingBasic deps)
    {
        int next = U_Util.CycleNext(deps.GoalPathPointIndex, deps.Direction, deps.PathPoints.Length, deps.DoesLoopPath);

        if (next < 0)
        {
            deps.Direction *= -1;
            next = U_Util.CycleNext(deps.GoalPathPointIndex, deps.Direction, deps.PathPoints.Length, deps.DoesLoopPath);
        }
        deps.GoalPathPointIndex = next;
        deps.GoalPathPoint = deps.PathPoints[deps.GoalPathPointIndex];
    }

    public static void ExecutePathing(this I_PathingBasic deps, float deltaTime)
    {
        deps.ActiveBehaviour(deltaTime);
    }
}
