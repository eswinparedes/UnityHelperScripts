using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_CharacterCoreMovement
{
    IObservable<CharacterMovementOutput> OnCharacterMovementOutput { get; }
}
