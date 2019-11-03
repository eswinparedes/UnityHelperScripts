public interface I_StateBehaviour
{
    void OnStateEnter();
    void OnStateExit();

    string StateName { get; }
}