using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using SUHScripts.Pending;
namespace SUHScripts
{
    using Functional;
    public static class CollisionObservableEffectOperations 
    {
        public static IObservable<CollisionObserved> OnEnterAny(
            this IEnumerable<A_ColliderObservable> @this) =>
            @this
            .Select(obs => obs.OnEnter)
            .Merge();

        public static IObservable<CollisionObserved> OnExitAny(
            this IEnumerable<A_ColliderObservable> @this) =>
            @this
            .Select(obs => obs.OnExit)
            .Merge();
        public static IObservable<Option<T>> OnEnterComponentOption<T>(this A_ColliderObservable @this) =>
            @this
            .OnEnter
            .Select(entered => entered.CollidingOther.GetComponentOption<T>());

        //SUHS TODO: Do the same for exit
        //SUHS TODO: Track the lifetime of (enter/exit) of an object
        public static IObservable<Option<T>> OnEnterQueryComponent<T>(this A_ColliderObservable @this) =>
            @this
            .OnEnter
            .Select(item => item.CollidingOther.QueryComponentOption<T>());

        public static IObservable<T> OnEnterQuerySome<T>(this A_ColliderObservable @this) =>
            @this
            .OnEnterQueryComponent<T>()
            .SelectSome();

        public static IObservable<T> OnEnterGetComponentSome<T>(this A_ColliderObservable @this) =>
            @this
            .OnEnterComponentOption<T>()
            .SelectSome();

        public static (IObservable<CollisionObserved> onEnter, IObservable<CollisionObserved> onExit)
            ObserveCollisions(Collider colliderToObserve)
        {
            var comp = colliderToObserve.gameObject.GetOrAddComponent<M_ColliderObservable>();
            return (comp.OnEnter, comp.OnExit);
        }

        public static IObservable<EnterExitable<CollisionObserved>> ObserveCollisionsGrouped(IReadOnlyList<Collider> collidersToObserve) =>
            Observable.Create<EnterExitable<CollisionObserved>>(observer =>
            {
                List<M_ColliderObservable> attachedObservables = new List<M_ColliderObservable>();

                for (int i = 0; i < collidersToObserve.Count(); i++)
                {
                    var colObservable = collidersToObserve[i].gameObject.GetOrAddComponent<M_ColliderObservable>();

                    attachedObservables.Add(colObservable);
                }

                var entered = attachedObservables.OnEnterAny();
                var exited = attachedObservables.OnExitAny();

                return OBV_EnterExit.EnterExitByCount(entered, exited, col => col.CollidingOther).Subscribe(observer);
            });
    }
}

