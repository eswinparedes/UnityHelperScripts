using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using SUHScripts.Functional;
using System.Threading;

namespace SUHScripts
{

    public static class State 
    {
        public enum EnterExitComposeOption
        {
            Prepend,
            Append,
            Infix,
            Outfix
        }

        public static Action EmptyAction { get; } = () => { };
        public static Action<float> EmptyFloatAction { get; } = d => { };

        public static IState EmptyState { get; } = new ComposableState(() => { }, () => { }, d => { });

        public static IState Append(this IState target, IState appendState) =>
        new ComposableState(
            () => { target.OnEnter(); appendState.OnEnter(); },
            () => { target.OnExit(); appendState.OnExit(); },
            dt => { target.Tick(dt); appendState.Tick(dt); });

        public static IState Prepend(this IState target, IState prependState) =>
            new ComposableState(
                () => { prependState.OnEnter(); target.OnEnter(); },
                () => { prependState.OnExit(); target.OnExit(); },
                dt => { prependState.Tick(dt); target.Tick(dt); });

        public static IState Append(this IState @this, Action onEnter = null, Action onExit = null, Action<float> tick = null) =>
            new ComposableState(
                onEnter == null ? (Action)@this.OnEnter : () => { @this.OnEnter(); onEnter(); },
                onExit == null ? (Action)@this.OnExit : () => { @this.OnExit(); onExit(); },
                tick == null ? (Action<float>)@this.Tick : dt => { @this.Tick(dt); tick(dt); });

        public static IState Logged(this IState @this, string prepend, bool tick = false) =>
            @this.Append(
                () => Debug.Log($"{prepend}: On Enter"),
                () => Debug.Log($"{prepend}: On Exit"),
                tick ? dt => Debug.Log($"{prepend}: Tick") : EmptyFloatAction);

        public static IState AppendAll(this IState @this, params IState[] appendStates)
        {
            IState state = @this;

            for (int i = 0; i < appendStates.Length; i++)
            {
                state = state.Append(appendStates[i]);
            }

            return state;
        }

        public static IState AppendAll(params IState[] appendStates)
        {
            IState state = appendStates[0];

            for (int i = 1; i < appendStates.Length; i++)
            {
                state = state.Append(appendStates[i]);
            }

            return state;
        }

        public static IState AppendAll(IReadOnlyList<IState> appendStates)
        {
            IState state = appendStates[0];

            for (int i = 1; i < appendStates.Count; i++)
            {
                state = state.Append(appendStates[i]);
            }

            return state;
        }

        public static IState EnterExitCompose(this IState @this, EnterExitComposeOption composition, Action onEnter = null, Action onExit = null)
        {
            switch (composition)
            {
                case EnterExitComposeOption.Prepend: return EnterExitPrepend(@this, onEnter, onExit);
                case EnterExitComposeOption.Append: return EnterExitAppend(@this, onEnter, onExit);
                case EnterExitComposeOption.Infix: return EnterExitInfix(@this, onEnter, onExit);
                case EnterExitComposeOption.Outfix: return EnterExitOutfix(@this, onEnter, onExit);
                default: return EnterExitCompose(@this, EnterExitComposeOption.Prepend, onEnter, onExit);
            }
        }

        public static IState EnterExitPrepend(this IState @this, Action onEnter = null, Action onExit = null)
        {
            Action enter = onEnter == null ? (Action)@this.OnEnter :
                () =>
                {
                    onEnter();
                    @this.OnEnter();
                };

            Action exit = onExit == null ? (Action)@this.OnExit :
                () =>
                {
                    onExit();
                    @this.OnExit();
                };

            return new ComposableState(enter, exit, @this.Tick);
        }

        public static IState EnterExitAppend(this IState @this, Action onEnter, Action onExit)
        {
            Action enter = onEnter == null ? (Action)@this.OnEnter :
                () =>
                {
                    @this.OnEnter();
                    onEnter();
                };

            Action exit = onExit == null ? (Action)@this.OnExit :
                () =>
                {
                    @this.OnExit();
                    onExit();
                };

            return new ComposableState(enter, exit, @this.Tick);
        }

        public static IState EnterExitOutfix(this IState @this, Action onEnter, Action onExit)
        {
            Action enter = onEnter == null ? (Action)@this.OnEnter :
                () =>
                {
                    onEnter();
                    @this.OnEnter();
                };

            Action exit = onExit == null ? (Action)@this.OnExit :
                () =>
                {
                    @this.OnExit();
                    onExit();
                };

            return new ComposableState(enter, exit, @this.Tick);
        }

        public static IState EnterExitInfix(this IState @this, Action onEnter, Action onExit)
        {
            Action enter = onEnter == null ? (Action)@this.OnEnter :
                () =>
                {
                    @this.OnEnter();
                    onEnter();
                };

            Action exit = onExit == null ? (Action)@this.OnExit :
                () =>
                {
                    onExit();
                    @this.OnExit();
                };

            return new ComposableState(enter, exit, @this.Tick);
        }

        public static IState ToState(this StateMachine @this, IState defaultState)
        {
            Action enter = () => @this.SetState(defaultState);
            Action exit = () => @this.SetState(null);
            Action<float> tick = t => @this.Tick(t);
            return new ComposableState(enter, exit, tick);
        }

        public static IState StateValved(params IValvable[] targets)
        {
            return new ComposableState(
                () => { for (int i = 0; i < targets.Length; i++) targets[i].SetValve(true); },
                () => { for (int i = 0; i < targets.Length; i++) targets[i].SetValve(false); },
                State.EmptyFloatAction);
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
