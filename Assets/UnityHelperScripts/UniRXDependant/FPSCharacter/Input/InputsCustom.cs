using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputsCustom : A_Inputs
{
    [Header("Deps")]
    [SerializeField] M_Interactor m_interactor = default;
    [SerializeField] FPSRoot m_root = default;
    [Header("Inputs")]
    [SerializeField] KeyCode m_jumpKey = KeyCode.Space;
    [SerializeField] KeyCode m_sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode m_dashKey = KeyCode.T;
    [SerializeField] KeyCode m_interactKey = KeyCode.E;
    [SerializeField] KeyCode m_ADSKey = KeyCode.Mouse1;
    [SerializeField] KeyCode m_fireKey = KeyCode.Mouse0;
    

    public override IObservable<MoveInputs> MoveInputs { get; protected set; }
    public override IObservable<Vector2> CameraLook { get; protected set; }
    
    public override IObservable<Unit> OnInteractStart { get; protected set; }
    public override IObservable<Unit> OnInteractEnd { get; protected set; }

    public override IObservable<Unit> OnFireStart { get; protected set; }
    public override IObservable<Unit> OnFireEnd { get; protected set; }

    public override IObservable<Unit> OnADSStart { get; protected set; }
    public override IObservable<Unit> OnADSEnd { get; protected set; }

    bool jumpPending = false;
    bool sprintPending = false;
    bool dashPending = false;

    BoolTrifecta jump = default;
    BoolTrifecta sprint = default;
    BoolTrifecta dash = default;


    public override void Initialize()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var canMove = true;

        m_interactor
            .InteractableStarted
            .Subscribe(interactableType =>
            {
                switch (interactableType)
                {
                    case InteractionType.Basic: break;
                    case InteractionType.LockView:
                        {
                            canMove = false;
                        } 
                        break;
                    default: throw new Exception("Pattern match not exhausted");
                }
            })
            .AddTo(this);

        m_interactor
            .InteractableEnded
            .Subscribe(_ =>
            {
                canMove = true;
            })
            .AddTo(this);

        m_root
        .OnUpdate
        .Where(_ => canMove)
        .Subscribe(tick =>
        {
            if (jumpPending)
            {
                jump = jump.AsTrueThisFrame();
            }
            else
            {
                if (Input.GetKeyDown(m_jumpKey))
                {
                    jumpPending = true;
                    jump = jump.AsTrueThisFrame();
                }
                else if (Input.GetKey(m_jumpKey))
                    jump = jump.AsTrueStay();
                else if (Input.GetKeyUp(m_jumpKey))
                    jump = jump.AsFalseThisFrame();
                else
                    jump = default;
            }

            if (sprintPending)
            {
                sprint = sprint.AsTrueThisFrame();
            }
            else
            {
                if (Input.GetKeyDown(m_sprintKey))
                {
                    sprintPending = true;
                    sprint = sprint.AsTrueThisFrame();
                }
                else if (Input.GetKey(m_sprintKey))
                    sprint = sprint.AsTrueStay();
                else if (Input.GetKeyUp(m_sprintKey))
                    sprint = sprint.AsFalseThisFrame();
                else
                    sprint = default;
            }

            if (dashPending)
            {
                dash = dash.AsTrueThisFrame();
            }
            else
            {
                if (Input.GetKeyDown(m_dashKey))
                {
                    dashPending = true;
                    dash = dash.AsTrueThisFrame();
                }
                else if (Input.GetKey(m_dashKey))
                    dash = dash.AsTrueStay();
                else if (Input.GetKeyUp(m_dashKey))
                    dash = dash.AsFalseThisFrame();
                else
                    dash = default;
            }
        })
        .AddTo(this);

        MoveInputs =
            this
            .FixedUpdateAsObservable()
            .Where(_ => canMove)
                .Select(_ =>
                {
                    var x = Input.GetAxis("Horizontal");
                    var y = Input.GetAxis("Vertical");
                    var vector = new Vector2(x, y).normalized;

                    jumpPending = false;
                    sprintPending = false;
                    dashPending = false;

                    return new MoveInputs(vector, jump, sprint, dash, Time.fixedDeltaTime);
                });

        CameraLook = 
            this
            .UpdateAsObservable()
            .Where(_ => canMove)
            .Select(_ =>
            {
                var x = Input.GetAxis("Mouse X");
                var y = Input.GetAxis("Mouse Y");
                return new Vector2(x, y);
            });

        OnFireStart =
            this
            .UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(m_fireKey));

        OnFireEnd =
            this
            .UpdateAsObservable()
            .Where(_ => Input.GetKeyUp(m_fireKey));

        OnInteractStart =
            this
            .UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(m_interactKey));

        OnInteractEnd =
            this
            .UpdateAsObservable()
            .Where(_ => Input.GetKeyUp(m_interactKey));

        OnADSStart =
            this
            .UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(m_ADSKey));

        OnADSEnd =
            this
            .UpdateAsObservable()
            .Where(_ => Input.GetKeyUp(m_ADSKey));
    }
}