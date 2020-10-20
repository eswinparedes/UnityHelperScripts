using System;

namespace SUHScripts.ReactiveFPS
{
    public class ComposableState<T> : ComposableState
    {
        public ComposableState(T sourceState, Action onEnter, Action onExit, Action<float> tick) : base(onEnter, onExit, tick)
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

        public ComposableState(Action onEnter, Action onExit, Action<float> tick)
        {
            m_onEnter = onEnter;
            m_onExit = onExit;
            m_tick = tick;
        }

        public void OnEnter() => m_onEnter();
        public void OnExit() => m_onExit();
        public void Tick(float deltaTime) => m_tick(deltaTime);
    }
}
