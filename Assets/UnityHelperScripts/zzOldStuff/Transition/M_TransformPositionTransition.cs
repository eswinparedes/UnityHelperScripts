using UnityEngine;

namespace SUHScripts
{
    public class M_TransformPositionTransition : A_Component
    {
        [Header("Transition Settings")]
        [SerializeField] Transform m_transitionTransform = default;
        [SerializeField] Transform m_enterPosition = default;
        [SerializeField] Transform m_exitPosition = default;
        [SerializeField] AnimationCurve m_movementCurve = default;
        [SerializeField] float m_transitionTime = .2f;

        Vector3 m_lerpPosA;
        Vector3 m_lerpPosB;
        Vector3 m_enterPos;
        Vector3 m_exitPos;
        C_Timer m_timer;

        private void Awake()
        {
            m_enterPos = m_enterPosition.localPosition;
            m_exitPos = m_exitPosition.localPosition;

            m_lerpPosA = m_transitionTransform.localPosition;
            m_lerpPosB = m_transitionTransform.localPosition;

            m_timer = new C_Timer();
            m_timer.SetTimerLength(m_transitionTime);
            m_timer.OnTimerComplete = OnTransitionEnd;
        }

        public override void Execute()
        {
            m_timer.UpdateTimer(m_componentManager.DeltaTime);
            m_transitionTransform.localPosition = Vector3.Lerp(m_lerpPosA, m_lerpPosB, m_movementCurve.Evaluate(m_timer.Alpha)); 
        }

        public void Enter()
        {
            m_componentManager.AddComponent(this);
            m_lerpPosA = m_exitPos;
            m_lerpPosB = m_enterPos;
            m_transitionTransform.localPosition = m_lerpPosA;
            m_timer.Restart();
        }

        public void Exit()
        {
            m_componentManager.AddComponent(this);
            m_lerpPosA = m_enterPos;
            m_lerpPosB = m_exitPos; 
            m_transitionTransform.localPosition = m_lerpPosA;
            m_timer.Restart();
        }

        public void OnTransitionEnd()
        {
            m_componentManager.RemoveComponent(this);
        }
    }
}

