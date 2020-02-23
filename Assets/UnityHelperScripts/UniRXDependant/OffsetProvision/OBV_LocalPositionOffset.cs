using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SUHScripts
{
    public class OBV_LocalPositionOffset : MonoBehaviour
    {
        public static IDisposable SubscribeLocalOffsetProvider(IObservable<Vector3> offsetProvider, Transform target)
        {
            var obvs = target.gameObject.GetOrAddComponent<PositionOffsetObserver>();
            return obvs.PushOffsetProvider(offsetProvider);
        }

        class PositionOffsetObserver : MonoBehaviour
        {
            Subject<IObservable<Vector3>> m_positionOffsetProvider = new Subject<IObservable<Vector3>>();

            public Vector3 SourceLocalOffset { get; set; } = Vector3.zero;

            private void Awake()
            {
                SourceLocalOffset = transform.localPosition;

                m_positionOffsetProvider.AddTo(this);

                m_positionOffsetProvider
                    .CombinedLatestScanner(
                    this.FixedUpdateAsObservable(),
                    (vectors, _) =>
                    {
                        var v = SourceLocalOffset;
                        foreach (var vec in vectors)
                        {
                            v += vec;
                        }

                        return v;
                    })
                    .Subscribe(offset => transform.localPosition = offset)
                    .AddTo(this);
            }

            public IDisposable PushOffsetProvider(IObservable<Vector3> provider)
            {
                var tempSub = new Subject<Vector3>();

                var tempSubDisp =
                    provider.Subscribe(tempSub)
                    .AddTo(this);

                m_positionOffsetProvider.OnNext(tempSub);

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
