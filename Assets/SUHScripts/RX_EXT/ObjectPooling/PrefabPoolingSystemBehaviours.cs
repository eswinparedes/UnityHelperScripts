using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace SUHScripts
{
    public static class PrefabPoolingSystemBehaviours 
    {
        public static (IObservable<Unit> OnSpawn, IObservable<Unit> OnDespawn) ObserveObjectPooling(this GameObject @this)
        {
            if (!PrefabPoolingSystem.GetIsObjectMangedByPool(@this))
            {
                Debug.LogError($"{@this.name} is not managed by pool and spawning cannot be observed");
                var obs = Observable.Empty<Unit>();
                return (obs, obs);
            }
            //SUHS TODO: THis could be optomized by added a fucntion that searches the hashset of prefab poolables managed
            var comp = @this.GetComponent<M_PoolableObservable>();
            return (comp.OnSpawn, comp.OnDespawn);
        }
    }
}