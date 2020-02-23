using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace SUHScripts
{
    public class zzTRANSFORM_SHAKE_TEST : MonoBehaviour
    {
        [SerializeField] Transform m_target0 = default;
        [SerializeField] Transform m_target1 = default;
        [SerializeField] TransformShakeSubscriber m_transformShakeSubscriber = default;

        Subject<SO_TransfromShakeData> m_shakes;
        Subject<INoiseGenerator> m_noises;

        private void Start()
        {
            m_shakes = new Subject<SO_TransfromShakeData>().AddTo(this);
            m_noises = new Subject<INoiseGenerator>().AddTo(this);

            m_transformShakeSubscriber
                .SubscribeTo(m_target0, M_UpdateManager.OnFixedUpdate_0, m_shakes)
                .AddTo(this);

            var result =
                this.UpdateAsObservable()
                .Select(_ => Time.deltaTime)
                .ObserveNoiseGenerators(m_noises, vs => vs.Sum())
                .Subscribe(v => m_target1.localPosition = v)
                .AddTo(this);
        }

        public void AddNoise(SO_A_NoiseData noise)
        {
            m_noises.OnNext(noise.Generator);
        }

       public void AddShake(SO_TransfromShakeData shakeData)
        {
            m_shakes.OnNext(shakeData);
        }
    }
}
