
public struct MovementRequest
{
    public readonly HorizontalMovement HorizontalMovement;
    public readonly VerticalMovement VerticalMovement;
    public readonly MovementRequestStatus RequestStatus;
    public readonly float deltaTime;

    public MovementRequest(
        HorizontalMovement horizontal, VerticalMovement vertical,
        MovementRequestStatus status, float deltaTime)
    {
        this.HorizontalMovement = horizontal;
        this.VerticalMovement = vertical;
        this.deltaTime = deltaTime;
        this.RequestStatus = status;
    }
}

