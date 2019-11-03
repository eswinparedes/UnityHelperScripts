using UnityEngine;
using System;

public class TransformPathing : A_Component
{
    [Header("Basic Pathing")]
    [SerializeField] Transform m_pathingTransform = default;
    [SerializeField] Rigidbody m_pathingRigid = default;
    [SerializeField] bool m_useRigidBody = default;
    [SerializeField] M_PathPoint[] m_pathPoints = default;
    [Header("Settings")]
    [SerializeField] Vector3 m_offsetMask = default;
    [SerializeField] float m_startDelay = default;
    [SerializeField] float m_pathPointDelay = default;
    [SerializeField] float m_pathingSpeed = default;
    [SerializeField] int m_startIndex = default;
    [SerializeField] [Range(-1, 1)] int m_direction = 1;
    [SerializeField]  bool m_doesLoopPath = default;
    [Header("Lerp Settings")]
    [SerializeField] AnimationCurve m_lerpCurve = default;

    public C_Timer LerpTimer { get; private set; }
    public C_Timer WaitTimer { get; private set; }

    public I_PathPoint GoalPathPoint { get; private set; }
    public I_PathPoint PreviousPathPoint { get; private set; }
    
    public Action<float> ActiveBehaviour { get; private set; }
    public Action<float> PathingBehaviour { get; private set; }
    public Action OnWaitCompleteBehaviour { get; private set; }
    public Action OnPathCompleteBehavoiut { get; private set; }

    public IndexCycle m_goalPathPointIndex { get; private set; }
    public Vector3 Offset { get; private set; }

    private void Awake()
    {
        InitializeLerp();    
    }

    public override void Execute()
    {
        ActiveBehaviour(m_componentManager.DeltaTime);
    }

    public void InitializeLerp()
    {
        m_goalPathPointIndex = new IndexCycle(m_startIndex, m_direction);
        GoalPathPoint = m_pathPoints[m_goalPathPointIndex.Index];

        Offset = -m_pathingTransform.GetOffsetTo(GoalPathPoint.PathPointPosition, m_offsetMask);

        WaitTimer = new C_Timer();
        WaitTimer.SetTimerLength(m_startDelay);
        WaitTimer.OnTimerComplete = () => WaitComplete();

        ActiveBehaviour = (deltaTime) => UpdateWaiting(deltaTime);
        PathingBehaviour = m_useRigidBody ? (Action<float>)UpdatePathingLerpRigidBody : UpdatePathingLerpTransform;

        PreviousPathPoint = GoalPathPoint;
        SetNextPathPoint();

        LerpTimer = new C_Timer();
        LerpTimer.SetTimerLength(m_pathingSpeed);
        LerpTimer.OnTimerComplete = () =>
        {
            PathingComplete();
            LerpTimer.Restart();
        };

        WaitTimer.OnTimerComplete = () =>
        {
            WaitComplete();
            ActiveBehaviour = (deltaTime) => PathingBehaviour(deltaTime);
        };
    }

    public void UpdatePathingLerpTransform(float deltaTime)
    {
        LerpTimer.UpdateTimer(deltaTime);

        float curve = m_lerpCurve.Evaluate(LerpTimer.Alpha);
        m_pathingTransform.position =
            Vector3.Lerp(PreviousPathPoint.PathPointPosition + Offset, GoalPathPoint.PathPointPosition + Offset, curve);
    }

    public void UpdatePathingLerpRigidBody(float deltaTime)
    {
        LerpTimer.UpdateTimer(deltaTime);

        float curve = m_lerpCurve.Evaluate(LerpTimer.Alpha);
        m_pathingRigid.MovePosition(Vector3.Lerp(PreviousPathPoint.PathPointPosition, GoalPathPoint.PathPointPosition, curve));
    }

    public void WaitComplete()
    {
        WaitTimer.SetTimerLength(m_pathPointDelay);
    }

    public void UpdateWaiting(float deltaTime)
    {
        WaitTimer.UpdateTimer(deltaTime);
    }

    public void PathingComplete()
    {
        ActiveBehaviour = (deltaTime) => UpdateWaiting(deltaTime);
        PreviousPathPoint = GoalPathPoint;
        SetNextPathPoint();
    }

    public void SetNextPathPoint()
    {
        m_goalPathPointIndex = m_goalPathPointIndex.CycleNext(m_pathPoints.Length, m_doesLoopPath);
        GoalPathPoint = m_pathPoints[m_goalPathPointIndex.Index];
    }
}
