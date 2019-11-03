
public struct CharacterMovementOutput
{
    public readonly CharacterControllerState OutputControllerState;
    public readonly MovementRequest OutputMovementRequest;

    public CharacterMovementOutput
        (CharacterControllerState outputState, MovementRequest outputRequest)
    {
        this.OutputMovementRequest = outputRequest;
        this.OutputControllerState = outputState;
    }
}
