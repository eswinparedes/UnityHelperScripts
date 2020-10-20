using SUHScripts.Functional;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace SUHScripts.ReactiveFPS
{
    public class AwaitableStateComposable : AwaitableState
    {
        Action m_onExitAsync;
        Func<CancellationToken, UniTask> m_onEnterBeginAsync;
        Action m_onEnterEnd;
        Action<float> m_tickWhenReady;

        public AwaitableStateComposable(Func<CancellationToken, UniTask> onEnterBeginAsync, Action onEnterEnd, Action onExitAsync, Action<float> tickWhenReady)
        {
            m_onExitAsync = onExitAsync;
            m_onEnterBeginAsync = onEnterBeginAsync;
            m_onEnterEnd = onEnterEnd;
            m_tickWhenReady = tickWhenReady;
        }

        protected override void OnExitAsync() => m_onExitAsync();

        protected override Option<UniTask> OnEnterBeginAsync() => m_onEnterBeginAsync(GetStateEnterCancellationToken()).AsOption_UNSAFE();

        protected override void OnEnterEnd() => m_onEnterEnd();

        protected override void TickWhenReady(float deltaTime) => m_tickWhenReady(deltaTime);
    }

    public class AwaitableStateComposable<T> : AwaitableStateComposable where T : IState
    {
        public T SourceState { get; }
        public AwaitableStateComposable(T sourceState, Func<CancellationToken, UniTask> onEnterBeginAsync, Action onEnterEnd, Action onExitAsync, Action<float> tickWhenReady)
            : base(onEnterBeginAsync, onEnterEnd, onExitAsync, tickWhenReady)
        {
            SourceState = sourceState;
        }
    }
}
