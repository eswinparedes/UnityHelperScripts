using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace SUHScripts.Tests
{
    public class TEST_FirstCompleted : MonoBehaviour
    {
        [SerializeField] KeyCode m_completeKey_1 = KeyCode.Alpha1;
        [SerializeField] KeyCode m_completeKey_2 = KeyCode.Alpha2;
        [SerializeField] KeyCode m_completeKey_3 = KeyCode.Alpha3;
        [SerializeField] KeyCode m_completeKey_4 = KeyCode.Alpha4;
        [SerializeField] KeyCode m_completeKey_5 = KeyCode.Alpha5;
        [SerializeField] KeyCode m_completeKey_6 = KeyCode.Alpha6;

        private void Awake()
        {
            this.WhenKey(m_completeKey_1).First()
                .FirstCompleted(this.WhenKey(m_completeKey_2).First(), t => t.ToString(), t => t.ToString())
                .FirstCompleted(this.WhenKey(m_completeKey_3).First(), t => t, t => t.ToString())
                .FirstCompleted(this.WhenKey(m_completeKey_4).First(), t => t, t => t.ToString())
                .FirstCompleted(this.WhenKey(m_completeKey_5).First(), t => t, t => t.ToString())
                .FirstCompleted(this.WhenKey(m_completeKey_6).First(), t => t, t => t.ToString())
                .Subscribe(onNext: k => Debug.Log(k), onCompleted: () => Debug.Log("When Key Complete"))
                .AddTo(this);
        }
    }

    public static class TEST_FirstCompletedExt
    {
        public static IObservable<KeyCode> WhenKey(this Component @this, KeyCode key) =>
            @this.UpdateAsObservable().Where(_ => Input.GetKeyDown(key)).Select(_ => key);
    }
}
