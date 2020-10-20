using SUHScripts.Functional;

namespace SUHScripts.ReactiveFPS
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Tick(float deltaTime);
    }
}
