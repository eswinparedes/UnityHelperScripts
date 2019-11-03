using UnityEngine;
using System;

public class M_AccelerationPathingTransform : A_Component, I_PathingAcceleration
{
    [Header("Basic Pathing")]
    [SerializeField] Transform m_pathingTransform = null;
    [SerializeField] Rigidbody m_pathingRigid = null;
    [SerializeField] bool m_useRigidBody = false;
    [SerializeField] M_PathPoint[] m_pathPoints = null;
    [Header("Settings")]
    [SerializeField] protected Vector3 m_offsetMask = Vector3.zero;
    [SerializeField] protected float m_startDelay = 0;
    [SerializeField] protected float m_pathPointDelay = 0;
    [SerializeField] protected float m_pathingSpeed = 5;
    [SerializeField] protected int m_startIndex = 0;
    [Range(-1, 1)] [SerializeField] protected int m_direction = 1;
    [SerializeField] protected bool m_loopPath = false;



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

    //Acceleration
    [Header("Acelleration Properties")]
    [SerializeField] float m_smoothTime = 0.1f;
    [SerializeField] float m_minDistance = 0.1f;

    Vector3 m_velocity = Vector3.zero;

    public float SmoothTime => m_smoothTime;
    public float MinDistance => m_minDistance;

    public Vector3 VelocityReference { get => m_velocity; set => m_velocity = value; }
    public Action<I_PathingAcceleration, float> PathingBehaviour { get; set; }
    public Action<I_PathingAcceleration> PathingCompleteBehaviour { get; set; }


    private void Awake()
    {
        this.Initialize();
    }

    public override void Execute()
    {
        this.ExecutePathing(m_componentManager.DeltaTime);
    }
}
