using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface I_PathingBasic
{
    Transform PathingTransform { get; }
    Rigidbody PathingRigidBody { get; }
    I_PathPoint[] PathPoints { get; }
    float PathingSpeed { get; }
    int Direction { get; set ; }
    bool DoesLoopPath { get; }
    int StartIndex { get; }
    Vector3 OffsetMask { get; }
    float StartDelay { get ; }
    float PathPointDelay { get; }
    C_Timer WaitTimer { get; set; }
    int GoalPathPointIndex { get; set; }
    I_PathPoint GoalPathPoint { get; set; }
    I_PathPoint PreviousPathPoint { get; set; }
    Vector3 Offset { get; set; }
    bool UseRigidBody { get ; }
    Action<float> ActiveBehaviour { get; set; }
}

public interface I_PathingLerp : I_PathingBasic
{
    C_Timer LerpTimer { get; set; }
    AnimationCurve LerpCurve { get; }
    Action<I_PathingLerp, float> PathingBehaviour { get; set; }
}

public interface I_PathingAcceleration : I_PathingBasic
{
    float SmoothTime { get; }
    float MinDistance { get; }
    Vector3 VelocityReference { get; set; }
    Action<I_PathingAcceleration, float> PathingBehaviour { get; set; }
    Action<I_PathingAcceleration> PathingCompleteBehaviour { get; set; }
}