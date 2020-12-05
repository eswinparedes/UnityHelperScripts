using SUHScripts.Functional;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace SUHScripts
{
    public abstract class AwaitableState : IState
    {
        public bool IsStateActive { get; private set; }

        CancellationTokenSource m_stateEnterCancellationTokenSource;
        protected CancellationToken GetStateEnterCancellationToken() => UniTaskExt.RefreshToken(ref m_stateEnterCancellationTokenSource);

        AwaitableStateStatus m_status;
        public async void OnEnter()
        {
            var opt = OnEnterBeginAsync();

            bool doRunEnterEnd = true;
            if (opt.IsSome)
            {
                m_status = AwaitableStateStatus.AwaitingEnter;
                doRunEnterEnd = !(await opt.Value.SuppressCancellationThrow());
            }

            if (doRunEnterEnd)
            {
                OnEnterEnd();
                IsStateActive = true;
                m_status = AwaitableStateStatus.Running;
            }
        }
         
        public void Tick(float deltaTime)
        {
            if (m_status != AwaitableStateStatus.Running) return;
            TickWhenReady(deltaTime);
        }

        public void OnExit()
        {
            m_stateEnterCancellationTokenSource.TryQuickCancelAndDispose();

            switch (m_status)
            {
                case AwaitableStateStatus.AwaitingEnter:
                    {
                        OnEnterEnd();
                    }break;
                case AwaitableStateStatus.Running:
                    {

                    }break;
            }
            
            OnExitAsync();
            m_status = AwaitableStateStatus.Exited;
            IsStateActive = false;
        }

        protected abstract void OnExitAsync();

        protected abstract Option<UniTask> OnEnterBeginAsync();
        protected abstract void OnEnterEnd();      
        protected abstract void TickWhenReady(float deltaTime);
    }

    enum AwaitableStateStatus
    {
        AwaitingEnter,
        Running,
        Exited
    }
}
