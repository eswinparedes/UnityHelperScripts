using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SUHScripts
{
    public class OBV_LocalPositionOffset : MonoBehaviour
    {
        public static IDisposable SubscribeLocalPositionOffsetProvider(IObservable<Vector3> offsetProvider, Transform target)
        {
            var obvs = target.gameObject.GetOrAddComponent<PositionOffsetObserver>();
            return obvs.PushPositionOffsetProvider(offsetProvider);
        }

        public static IDisposable SubscribeLocalEulerOffsetProvider(IObservable<Vector3> offsetProvider, Transform target)
        {
            var obvs = target.gameObject.GetOrAddComponent<PositionOffsetObserver>();
            return obvs.PushEulerOffsetProvider(offsetProvider);
        }

        class PositionOffsetObserver : MonoBehaviour
        {
            Subject<IObservable<Vector3>> m_localPositionOffsetProvider = new Subject<IObservable<Vector3>>();
            Subject<IObservable<Vector3>> m_localEulerOffsetProvider = new Subject<IObservable<Vector3>>();
            public Vector3 SourceLocalPositionOffset { get; set; } = Vector3.zero;
            public Vector3 SourceLocalEulerOffset { get; set; }
            private void Awake()
            {
                SourceLocalPositionOffset = transform.localPosition;

                m_localPositionOffsetProvider.AddTo(this);

                m_localPositionOffsetProvider
                    .ReduceLatestBy(this.FixedUpdateAsObservable(), (vectors, _) => vectors.Average())
                    .Subscribe(offset => transform.localPosition = SourceLocalPositionOffset + offset)
                    .AddTo(this);

                SourceLocalEulerOffset = transform.localEulerAngles;

                m_localEulerOffsetProvider
                    .ReduceLatestBy(this.FixedUpdateAsObservable(), (vectors, _) => vectors.Average())
                    .Subscribe(offset => transform.localEulerAngles = SourceLocalEulerOffset + offset)
                    .AddTo(this);
            }

            public IDisposable PushPositionOffsetProvider(IObservable<Vector3> provider)
            {
                var tempSub = new Subject<Vector3>();

                var tempSubDisp =
                    provider.Subscribe(tempSub)
                    .AddTo(this);

                m_localPositionOffsetProvider.OnNext(tempSub);

                return Disposable.Create(() =>
                {
                    tempSubDisp.Dispose();
                    tempSub.OnCompleted();
                    tempSub.Dispose();
                });
            }

            public IDisposable PushEulerOffsetProvider(IObservable<Vector3> provider)
            {
                var tempSub = new Subject<Vector3>();

                var tempSubDisp =
                    provider.Subscribe(tempSub)
                    .AddTo(this);

                m_localEulerOffsetProvider.OnNext(tempSub);

                return Disposable.Create(() =>
                {
                    tempSubDisp.Dispose();
                    tempSub.OnCompleted();
                    tempSub.Dispose();
                });
            }
        }
    }

}
