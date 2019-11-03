using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class M_LerpPathingTransform : A_Component, I_PathingLerp
{
    [Header("Basic Pathing")]
    [SerializeField] Transform m_pathingTransform = default;
    [SerializeField] Rigidbody m_pathingRigid = default;
    [SerializeField] bool m_useRigidBody = default;
    [SerializeField] M_PathPoint[] m_pathPoints = default;
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
    public Action< float> ActiveBehaviour { get; set; }    
    public Action<I_PathingBasic> OnWaitCompleteBehaviour { get; set; }
    public Action<I_PathingBasic> OnPathCompleteBehavoiut { get; set; }

    //Lerp Properties
    [Header("Lerp Settings")]
    [SerializeField] AnimationCurve m_lerpCurve = new AnimationCurve();

    public C_Timer LerpTimer { get; set; }
    public AnimationCurve LerpCurve { get => m_lerpCurve; }

    public Action<I_PathingLerp, float> PathingBehaviour { get; set; }

    private void Awake()
    {
        this.InitializeLerp();
    }

    public override void Execute()
    {
        this.ExecutePathing(m_componentManager.DeltaTime);
    }
}
