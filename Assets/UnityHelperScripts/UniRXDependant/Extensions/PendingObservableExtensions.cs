using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SUHScripts.Pending
{
    public static class PendingObservableExtensions 
    {
        
       

        public static IObservable<Unit> UpdateAsObservable(this Component @this) =>
            ObservableTriggerExtensions.UpdateAsObservable(@this).Where(_ => Time.timeScale > 0);

        public static IObservable<Unit> FixedUpdateAsObservable(this Component @this) =>
            ObservableTriggerExtensions.FixedUpdateAsObservable(@this).Where(_ => Time.timeScale > 0);
    }   
}