using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class M_TransformShake : MonoBehaviour
{
    [SerializeField] TransformShakeSubscriber m_transformShakeSubscriber = default;

    Subject<SO_TransfromShakeData> m_shakes;

    private void Start()
    {
        m_shakes = new Subject<SO_TransfromShakeData>().AddTo(this);

        m_transformShakeSubscriber
            .SubscribeTo(M_UpdateManager.OnFixedUpdate_0, m_shakes)
            .AddTo(this);
    }

   public void AddShake(SO_TransfromShakeData shakeData)
    {
        m_shakes.OnNext(shakeData);
    }
}