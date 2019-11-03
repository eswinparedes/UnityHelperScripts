using UnityEngine;

public struct MoveInputs
{
    public readonly Vector2 localMovement;
    public readonly BoolTrifecta jumpState;
    public readonly BoolTrifecta sprint;
    public readonly BoolTrifecta dash;
    public readonly float deltaTime;

    public MoveInputs(Vector2 movement, BoolTrifecta jump, BoolTrifecta sprint, BoolTrifecta dash, float deltaTime)
    {
        this.localMovement = movement;
        this.jumpState = jump;
        this.sprint = sprint;
        this.deltaTime = deltaTime;
        this.dash = dash;
    }
}
