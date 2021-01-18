using SUHScripts.Functional;

namespace SUHScripts
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Tick(float deltaTime);
    }
}
