using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CharacterMovement 
{
    public delegate CharacterMovementOutput CharacterMovementFunction(MoveInputs inputs);

    public static CharacterMovementFunction BeginBuild<T>(CharacterMovementOutput outputSeed, T cacheSeed) =>
        inputs => default;

}
