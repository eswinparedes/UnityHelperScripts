using UnityEngine;
using SUHScripts;
using System;
using UnityEngine.Events;
using UniRx.Triggers;
using UniRx;

namespace RXUI
{
    public abstract class AEnterExitObservable : MonoBehaviour, IEnterExitObservable
    {
        [Header("Enter Events")]
        [SerializeField] UnityEvent m_onEnter = default;
        [SerializeField] UnityEvent m_onExit = default;
        [Header("Transition Events")]
        [SerializeField] float m_enterExitLength = default;
        [SerializeField] AnimationCurve m_enterCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] AnimationCurve m_exitCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        Subject<bool> m_enterExitSubject = new Subject<bool>();

        public IObservable<bool> EnterExitStream => m_enterExitSubject;

        public IObservable<(float alpha, FTimer progress)> Progress =>
            EnterExitStream.TimerPingPongByEmission(this.UpdateAsObservable().Select(_ => Time.deltaTime), m_enterExitLength)
            .Select(timer =>
            {
                if (timer.IsIncrementing)
                {
                    return (m_enterCurve.Evaluate(timer.TimeAlpha()), timer);
                }
                else
                {
                    return (m_exitCurve.Evaluate(timer.TimeAlpha()), timer);
                }
            });

        protected void RaiseEnter() => m_enterExitSubject.OnNext(true);
        protected void RaiseExit() => m_enterExitSubject.OnNext(false);

        protected virtual void Awake()
        {
            EnterExitStream.Subscribe(isEnter =>
            {
                if (isEnter) m_onEnter.Invoke();
                else m_onExit.Invoke();
                //Debug.Log($"Is enter {isEnter} on {gameObject.name}");
            }).AddTo(this);
        }
    }
}
