using System;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace SUHScripts
{
    public class AwaitableStateEnterGroup
    {
        public AwaitableStateEnterGroup(Func<CancellationToken, UniTask> creationSource, Action onEnterEnd)
        {
            OnEnterBeginAsync = creationSource;
            OnEnterEnd = onEnterEnd;
        }

        public Func<CancellationToken, UniTask> OnEnterBeginAsync { get; }
        public Action OnEnterEnd { get; }
    }
}

