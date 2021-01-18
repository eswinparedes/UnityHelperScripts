using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace RXUI
{
    public class _EnterExitUISimple : MonoBehaviour, IEnterExitObserver
    {
        [Header("Effects")]
        [SerializeField] CanvasGroup m_canvasGroup = default;
        [SerializeField] RectTransform m_enterTarget = default;
        [SerializeField] RectTransform m_exitTarget = default;
        [SerializeField] Vector3 m_enterScale = Vector3.one;
        [SerializeField] Vector3 m_exitScale = Vector3.zero;
        [SerializeField] bool m_startsEntered = default;

        private void Awake()
        {
            if (m_startsEntered)
            {
                m_canvasGroup.alpha = 1;
                m_canvasGroup.transform.position = m_enterTarget.position;
                m_canvasGroup.transform.localScale = m_enterScale;
                m_canvasGroup.blocksRaycasts = true;
            }
            else
            {
                m_canvasGroup.alpha = 0;
                ((RectTransform) m_canvasGroup.transform).anchoredPosition = m_exitTarget.anchoredPosition;
                m_canvasGroup.transform.localScale = m_exitScale;
                m_canvasGroup.blocksRaycasts = false;
            }
        }
        public void ObserveEnterExitCommands(IEnterExitObservable enterExitCommands)
        {
            enterExitCommands.EnterExitStream.Subscribe(isEnter =>
            {
                m_canvasGroup.blocksRaycasts = isEnter;
            }).AddTo(this);

            enterExitCommands.Progress.Subscribe(progress =>
            {
                m_canvasGroup.alpha = progress.alpha;
                m_canvasGroup.transform.position = Vector3.Lerp(m_exitTarget.position, m_enterTarget.position, progress.alpha);
                m_canvasGroup.transform.localScale = Vector3.Lerp(m_exitScale, m_enterScale, progress.alpha);

            }).AddTo(this);

        }

    }
}

