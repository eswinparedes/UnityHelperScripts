using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class M_LerpEllipsePathingTransform :A_Component, I_PathingLerp
{
    [Header("Basic Pathing")]
    [SerializeField] Transform m_pathingTransform = default;
    [SerializeField] Rigidbody m_pathingRigid = default;
    [SerializeField] bool m_useRigidBody = default;

    [Header("Settings")]
    [SerializeField] protected Vector3 m_offsetMask = default;
    [SerializeField] protected float m_startDelay = default;
    [SerializeField] protected float m_pathPointDelay = default;
    [SerializeField] protected float m_pathingSpeed = default;
    [SerializeField] protected int m_startIndex = default;
    [Range(-1, 1)] [SerializeField] protected int m_direction = 1;
    [SerializeField] protected bool m_loopPath = default;



    public Transform PathingTransform { get => m_pathingTransform; }
    public Rigidbody PathingRigidBody { get => m_pathingRigid; }
    public I_PathPoint[] PathPoints { get => m_pathPoints; }
    public float PathingSpeed { get => m_pathingSpeed; }
    public int Direction { get => m_direction; set => m_direction = value; }
    public bool DoesLoopPath { get => m_loopPath; }
    public int StartIndex { get => m_startIndex; }
    public Vector3 OffsetMask { get => m_offsetMask; }
    public float StartDelay { get => m_startDelay; }
    public float PathPointDelay { get => m_pathPointDelay; }
    public C_Timer WaitTimer { get; set; }
    public int GoalPathPointIndex { get; set; }
    public I_PathPoint GoalPathPoint { get; set; }
    public I_PathPoint PreviousPathPoint { get; set; }
    public Vector3 Offset { get; set; }
    public bool UseRigidBody { get => m_useRigidBody; }
    public Action<float> ActiveBehaviour { get; set; }
    public Action<I_PathingBasic> OnWaitCompleteBehaviour { get; set; }
    public Action<I_PathingBasic> OnPathCompleteBehavoiut { get; set; }

    //Lerp Properties
    [Header("Lerp Settings")]
    [SerializeField] AnimationCurve m_lerpCurve = default;

    public C_Timer LerpTimer { get; set; }
    public AnimationCurve LerpCurve { get => m_lerpCurve; }

    public Action<I_PathingLerp, float> PathingBehaviour { get; set; }

    //Ellipse
    [Header("Elipse Properties")]
    [SerializeField] Ellipse m_ellipse = default;
    [SerializeField] Transform m_centerTransform = default;

    public I_PathPoint[] m_pathPoints = default;

    public void Awake()
    {
        m_ellipse.CalculateElipse();
        Vector3 pos = m_centerTransform.position;
        m_pathPoints = m_ellipse.EllipsePoints.Select(x => new C_PathPoint((Vector3)x + pos)).ToArray();

        this.InitializeLerp();
    }

    public override void Execute()
    {
        this.ExecutePathing(m_componentManager.DeltaTime);
    }
}
