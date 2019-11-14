using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class M_TransformShake : MonoBehaviour
{
    [SerializeField] Transform m_target0;
    [SerializeField] Transform m_target1;
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

        var result = ShakeOperations.SubscribeTo(M_UpdateManager.OnFixedUpdate_0, m_noises);
        result.subscription.AddTo(this);
        result.noiseFunction.Subscribe(noise => m_target1.localPosition = noise).AddTo(this);
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