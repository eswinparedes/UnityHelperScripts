using UnityEngine;

namespace SUHScripts
{
    public class M_TransformScaleTransition : A_Component
    {
        [SerializeField] TransitionScale m_transitionScale = default;

        private void Start()
        {
            m_transitionScale.Start();
        }
        public override void Execute()
        {
            m_transitionScale.Update(m_componentManager.DeltaTime);
        }

        public void Enter() => m_transitionScale.Enter();
        public void Exit() => m_transitionScale.Exit();
    }
}

