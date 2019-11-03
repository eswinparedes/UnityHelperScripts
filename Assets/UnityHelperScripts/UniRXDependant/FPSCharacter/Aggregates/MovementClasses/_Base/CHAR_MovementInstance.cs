using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class CHAR_MovementInstance 
{
    public delegate CharacterMovementOutput CharacterMovementFunction(MoveInputs inputs);
    public delegate bool CanEnterValidation();

    public abstract CharacterMovementFunction ProcessMovement { get; protected set; }
    public abstract CanEnterValidation CanEnter { get; protected set; }
    public abstract Action Enter { get; protected set; }
    public abstract Action Exit { get; protected set; }

    //CODE SMELL
    public abstract void PushState(CharacterMovementOutput state);
    public abstract CharacterMovementOutput GetState();

    public CHAR_MovementInstance OnEnterAppend(Action action)
    {
        Action oldEnter = Enter.Clone() as Action;
        Enter = () =>
        {
            oldEnter();
            action();
        };

        return this;
    }

    public CHAR_MovementInstance OnExitAppend(Action action)
    {
        Action oldExit = Exit.Clone() as Action;
        Exit = () =>
        {
            oldExit();
            action();
        };

        return this;
    }

    public CHAR_MovementInstance WithValidation(Func<bool> validation)
    {
        var oldvalid = CanEnter;
        CanEnter =
            () =>
            {
                if (!oldvalid())
                    return false;

                return validation();
            };

        return this;
    }

}

