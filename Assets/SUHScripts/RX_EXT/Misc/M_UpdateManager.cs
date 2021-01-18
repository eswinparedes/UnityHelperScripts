using System;
using UniRx;
using UnityEngine;
using UniRx.Triggers;


namespace SUHScripts
{
    using Functional;
    using static Functional.Functional;

    public class M_UpdateManager : MonoBehaviour
    {
        static Subject<float> m_onUpdate_0 = new Subject<float>();
        static Subject<float> m_onUpdate_1 = new Subject<float>();
        static Subject<float> m_onUpdate_2 = new Subject<float>();

        static Subject<float> m_onFixedUpdate_0 = new Subject<float>();
        static Subject<float> m_onFixedUpdate_1 = new Subject<float>();
        static Subject<float> m_onFixedUpdate_2 = new Subject<float>();

        public static IObservable<float> OnUpdate_0 => m_onUpdate_0;
        public static IObservable<float> OnUpdate_1 => m_onUpdate_1;
        public static IObservable<float> OnUpdate_2 => m_onUpdate_2;

        public static IObservable<float> OnFixedUpdate_0 => m_onUpdate_0;
        public static IObservable<float> OnFixedUpdate_1 => m_onUpdate_1;
        public static IObservable<float> OnFixedUpdate_2 => m_onUpdate_2;

        static Option<M_UpdateManager> Instance = None.Default;

        private void Awake()
        {
            Instance
                .ForEachPass(inst => Debug.LogError($"Update Manager Instance already set on {inst.gameObject.name}, rejecting instance on {gameObject.name}"))
                .ForNonePass(() => Debug.Log($"Attaching Update Manager Instance on {gameObject.name}"))
                .WhenNone(this);

            m_onUpdate_0.AddTo(this);
            m_onUpdate_1.AddTo(this);
            m_onUpdate_2.AddTo(this);

            this
                .UpdateAsObservable()
                .Subscribe(_ =>
                {
                    m_onUpdate_0.OnNext(Time.deltaTime);
                    m_onUpdate_1.OnNext(Time.deltaTime);
                    m_onUpdate_2.OnNext(Time.deltaTime);
                })
                .AddTo(this);

            m_onFixedUpdate_0.AddTo(this);
            m_onFixedUpdate_1.AddTo(this);
            m_onFixedUpdate_2.AddTo(this);

            this
                .FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    m_onFixedUpdate_0.OnNext(Time.fixedDeltaTime);
                    m_onFixedUpdate_1.OnNext(Time.fixedDeltaTime);
                    m_onFixedUpdate_2.OnNext(Time.fixedDeltaTime);
                })
                .AddTo(this);
        }
    }
}

