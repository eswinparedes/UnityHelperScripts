using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static CHAR_MovmentInstanceExt;

[System.Serializable]
public class GroundedMovement : I_CharacterCoreMovement
{
    [SerializeField] FPSRoot m_root = default;
    [Header("States")]
    [SerializeField] CHAR_MovementSource m_defaultMovement = default;
    [SerializeField] CHAR_MovementSource m_ADSMovement = default;
    [SerializeField] CHAR_MovementSource m_specialMovement = default;

    public Subject<CharacterMovementOutput> m_onCharacterMovementOuptut;
    public IObservable<CharacterMovementOutput> OnCharacterMovementOutput =>
        m_onCharacterMovementOuptut;

    ///Make camera output where the camera is looking
    public void Subscribe()
    {
        m_onCharacterMovementOuptut =
            new Subject<CharacterMovementOutput>();

        /* have a consumable that you can request a consume????
         * We would have a consume max value and a consume amount value
         * and simply request a consume and get a rejection if the amount requested was 
         * rejected
         * */
        //Movement
        var defaultMovement =
            m_defaultMovement
            .Build(m_root)
            .OnEnterAppend(() => m_root.FPSCamera.RequestResetFOV());

        var specialMovement =
            m_specialMovement
            .Build(m_root)
            .OnEnterAppend(() => m_root.FPSCamera.RequestSetFOV(80, .075f));

        var adsMovement =
            m_ADSMovement
            .Build(m_root)
            .OnEnterAppend(() => m_root.FPSCamera.RequestSetFOV(35));

        var MUTABLE_ACTIVE_MOVEMENT = defaultMovement;
        //State Transition Validations
        var adsCanProceed = NoneOfTheseStates(specialMovement, adsMovement);

        //Staet Transtiions
        Action<CHAR_MovementInstance> RequestState =
            next => 
                MUTABLE_ACTIVE_MOVEMENT = MUTABLE_ACTIVE_MOVEMENT.ReturnTransitioned(next);
        
        //Camera + turning
        var cameraFunction =
            FPSStatefulFunctions
            .StandardCameraUpdateMovement_SideEffecting(m_root, Vector3.up);

        //SUBSCRIPTONS
        m_root
            .Inputs
            .MoveInputs
            .Subscribe(inputs =>
            {
                var output = MUTABLE_ACTIVE_MOVEMENT.ProcessMovement(inputs);

                m_onCharacterMovementOuptut.OnNext(output);

                if (output.OutputMovementRequest.IsTerminated())
                {
                    RequestState(defaultMovement);
                    m_root.FPSCamera.RequestResetFOV();
                }
            })
            .AddTo(m_root.AttachBehaviour);

        //ADS SUBS
        m_root
            .Inputs
            .OnADSStart
            .Where(_ => adsCanProceed(MUTABLE_ACTIVE_MOVEMENT))
            .Subscribe(_ => RequestState(adsMovement))
            .AddTo(m_root.AttachBehaviour);

        m_root
            .Inputs
            .OnADSEnd
            .Where(_ => MUTABLE_ACTIVE_MOVEMENT == adsMovement)
            .Subscribe(_ => RequestState(defaultMovement))
            .AddTo(m_root.AttachBehaviour);

        //DASH SUBS
        m_root
            .Inputs
            .OnInteractStart
            .Where(_ => MUTABLE_ACTIVE_MOVEMENT != specialMovement)
            .Subscribe(_ => RequestState(specialMovement))
            .AddTo(m_root.AttachBehaviour);

        //Camera
        m_root
            .Inputs
            .CameraLook
            .Subscribe(input => cameraFunction(input))
            .AddTo(m_root.AttachBehaviour);

        m_root.FPSSignals.RequestNewCharacterCoreMovementSource(this);
    }

    
}
