using UnityEngine;
using  SUHScripts.Functional;

public static class DomainExtensions 
{
    

    /// <summary>
    /// IMPURE FUNCTION MOVES CHARACTER AND OUTPUTS AN UPDATED STRUCT
    /// </summary>
    /// <param name="this"></param>
    /// <param name="lastState"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public static CharacterMovementOutput MoveAndOutput
        (this CharacterController @this, CharacterMovementOutput lastState, MovementRequest request)
    {
        @this.Move(request.TotalMovement() * request.deltaTime);
        var newState =
            @this.ExtractState(lastState.OutputControllerState, true);
        return new CharacterMovementOutput(newState, request);
    }

    public static SphereCastData CharacterGroundingCast(this CharacterController @this,
        int layerMask = ~0, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
    {
        var direction = @this.transform.TransformDirection(Vector3.down);
        return PhysicsCasting.SphereCast(
            @this.transform.position, @this.radius, direction, @this.height, layerMask, queryTriggerInteraction);
    }
}
