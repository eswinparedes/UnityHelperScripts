using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using SUHScripts.Functional;
using System.Threading;

namespace SUHScripts.ReactiveFPS
{
    public static class StateOperations 
    {
        public static IState ToState(this StateMachine @this, IState defaultState)
        {
            Action enter = () => @this.SetState(defaultState);
            Action exit = () => @this.SetState(null);
            Action<float> tick = t => @this.Tick(t);
            return new ComposableState(enter, exit, tick);
        }

        public static ComposableState<T> GetBoundState<T>(this T @this, Action onEnter = null, Action onExit = null, Action<float> tick = null) where T : IState
        {
            Action _onEnter = onEnter == null ? (Action)@this.OnEnter : () => { @this.OnEnter(); onEnter(); };
            Action _onExit = onExit == null ? (Action)@this.OnExit : () => { @this.OnExit(); onExit(); };
            Action<float> _tick = tick == null ? (Action<float>)@this.Tick : t => { @this.Tick(t); tick(t); };
            return new ComposableState<T>(@this, _onEnter, _onExit, _tick);
        }

        public static AwaitableStateComposable<T> GetBoundStateAwaitable<T>(this T @this, Func<CancellationToken, UniTask> onEnterBeginAsync, Action onEnterEnd = null, Action onExit = null, Action<float> tick = null) where T : IState
        {
            Action _onEnterEnd = onEnterEnd == null ? (Action)@this.OnEnter : () => { onEnterEnd();  @this.OnEnter();};
            Action _onExit = onExit == null ? (Action)@this.OnExit : () => { @this.OnExit(); onExit(); };
            Action<float> _tick = tick == null ? (Action<float>)@this.Tick : t => { @this.Tick(t); tick(t); };

            return new AwaitableStateComposable<T>(@this, onEnterBeginAsync, _onEnterEnd, _onExit, _tick); 
        }

        public static AwaitableStateComposable<T> GetBoundStateAwaitable<T>(this T @this, AwaitableStateEnterGroup group, Action onExit = null, Action<float> tick = null) where T : IState
        {
            Action _onEnterEnd = () => { group.OnEnterEnd(); @this.OnEnter(); };
            Action _onExit = onExit == null ? (Action)@this.OnExit : () => { @this.OnExit(); onExit(); };
            Action<float> _tick = tick == null ? (Action<float>)@this.Tick : t => { @this.Tick(t); tick(t); };

            return new AwaitableStateComposable<T>(@this, group.OnEnterBeginAsync, _onEnterEnd, _onExit, _tick);
        }

    }

}
