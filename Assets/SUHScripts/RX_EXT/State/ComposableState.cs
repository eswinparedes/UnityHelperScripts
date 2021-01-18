using System;

namespace SUHScripts
{
    public class ComposableState<T> : ComposableState
    {
        public ComposableState(T sourceState, Action onEnter = null, Action onExit = null, Action<float> tick = null) : base(onEnter, onExit, tick)
        {
            SourceState = sourceState;
        }

        public T SourceState { get; }
    }

    public class ComposableState : IState
    {
        Action m_onEnter;
        Action m_onExit;
        Action<float> m_tick;

        public ComposableState(Action onEnter = null, Action onExit = null, Action<float> tick = null)
        {
            m_onEnter = onEnter ?? State.EmptyAction;
            m_onExit = onExit ?? State.EmptyAction;
            m_tick = tick ?? State.EmptyFloatAction;
        }

        public void OnEnter() => m_onEnter();
        public void OnExit() => m_onExit();
        public void Tick(float deltaTime) => m_tick(deltaTime);
    }
}
