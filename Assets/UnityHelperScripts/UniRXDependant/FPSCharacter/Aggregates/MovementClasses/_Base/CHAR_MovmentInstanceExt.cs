using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CHAR_MovmentInstanceExt
{ 
    public static CHAR_MovementInstance ReturnTransitioned(this CHAR_MovementInstance @this, CHAR_MovementInstance next)
    {
        if (next.CanEnter())
        {
            @this.Exit();
            var lastState = @this.GetState();
            next.PushState(lastState);
            next.Enter();
            return next;
        }

        return @this;
    }

    public static Func<CHAR_MovementInstance, bool> NoneOfTheseStates(params CHAR_MovementInstance[] states)
    {
        var hashSet = new HashSet<CHAR_MovementInstance>(states);
        return state => !hashSet.Contains(state);
    }
}
