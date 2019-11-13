using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;

public class M_InteractableFocus : MonoBehaviour
{
    [SerializeField] A_InteractableObservable m_interactableEvents = default;
    [SerializeField] Transform m_root = default;
    [SerializeField] float m_focusDistance = .1f;
    [SerializeField] float m_lerpSpeed = 15f;

    private void Start()
    {
        Vector3 startPos = m_root.position;
        Quaternion startRot = m_root.rotation;

        Option<I_Interactor> current = NONE;

        m_interactableEvents
            .InteractStart
            .Where(interactor => !current.ComparesTo(interactor))
            .Subscribe(interactor => current = interactor.AsOption())
            .AddTo(this);

        m_interactableEvents
            .InteractEnd
            .Where(interactor => current.ComparesTo(interactor))
            .Subscribe(_ => current = NONE)
            .AddTo(this);

        M_UpdateManager
            .OnUpdate_0
            .Where(tick => current.IsSome)
            .Subscribe(tick =>
            {
                var targetPos =
                    current.Value.InteractorData.Position + current.Value.InteractorData.Forward() * m_focusDistance;

                var offset =
                    current.Value.InteractorData.Position - m_root.position;

                var dir = offset.normalized;

                var targetRot = Quaternion.LookRotation(dir);
                m_root.position = Vector3.Lerp(m_root.position, targetPos, m_lerpSpeed * tick);
                m_root.rotation = Quaternion.Lerp(m_root.rotation, targetRot, m_lerpSpeed * tick);
            })
            .AddTo(this);

        M_UpdateManager
            .OnUpdate_0
            .Where(tick => !current.IsSome)
            .Subscribe(tick =>
            {
                m_root.position = Vector3.Lerp(m_root.position, startPos, m_lerpSpeed * tick);
                m_root.rotation = Quaternion.Lerp(m_root.rotation, startRot, m_lerpSpeed * tick);
            })
            .AddTo(this);
    }
}
