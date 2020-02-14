using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

namespace SUHScripts
{
    using Functional;
    public static class CollisionObservableEffectOperations 
    {
        public static IObservable<ICollisionObservation> OnEnterAny(
            this IEnumerable<A_ColliderObservable> @this) =>
            @this
            .Select(obs => obs.OnEnter)
            .Merge();

        public static IObservable<ICollisionObservation> OnExitAny(
            this IEnumerable<A_ColliderObservable> @this) =>
            @this
            .Select(obs => obs.OnExit)
            .Merge();

        public static bool Any(this Dictionary<Collider, int> @this, Collider col)
        {
            if (@this.ContainsKey(col))
            {
                return @this[col] > 0;
            }
            else
            {
                return false;
            }
        }
        

        public static void Increment(this Dictionary<Collider, int> @this, Collider col)
        {
            if (@this.ContainsKey(col))
            {
                @this[col] = @this[col] + 1;
            }
            else
            {
                @this.Add(col, 1);
            }
        }

        public static void Decrement(this Dictionary<Collider, int> @this, Collider col)
        {
            if (@this.ContainsKey(col))
            {
                @this[col] = @this[col] - 1;

                if (@this[col] <= 0)
                {
                    @this.Remove(col);
                }
            }
        }

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

        public static (IObservable<ICollisionObservation> onEnter, IObservable<ICollisionObservation> onExit)
            ObserveCollisions(Collider colliderToObserve)
        {
            var comp = colliderToObserve.gameObject.GetOrAddComponent<M_ColliderObservable>();
            return (comp.OnEnter, comp.OnExit);
        }

        public static (IObservable<ICollisionObservation> onEnter, IObservable<ICollisionObservation> onExit)
            ObserveCollisionsGrouped(IReadOnlyList<Collider> collidersToObserve, Component disposalTarget)
        {
            List<M_ColliderObservable> attachedObservables = new List<M_ColliderObservable>();

            for(int i = 0; i < collidersToObserve.Count(); i++)
            {
                var colObservable =
                    collidersToObserve[i].gameObject.GetOrAddComponent<M_ColliderObservable>();

                attachedObservables.Add(colObservable);
            }

            var onEnterSubject = new Subject<ICollisionObservation>();
            var onExitSubject = new Subject<ICollisionObservation>();
            var collisionsSustained = new Dictionary<Collider, int>();

            var anyEnterObserved =
                attachedObservables
                .OnEnterAny()
                .Subscribe(output =>
                {
                    if (!collisionsSustained.Any(output.CollidingOther))
                        onEnterSubject.OnNext(output);

                    collisionsSustained.Increment(output.CollidingOther);

                })
                .AddTo(disposalTarget);

            var anyExitObserved =
                attachedObservables
                .OnExitAny()
                .Subscribe(output =>
                {
                    collisionsSustained.Decrement(output.CollidingOther);

                    if (!collisionsSustained.Any(output.CollidingOther))
                        onExitSubject.OnNext(output);

                })
                .AddTo(disposalTarget);

            return (onEnterSubject, onExitSubject);
        }

        public static (IObservable<ICollisionObservation> onEnter, IObservable<ICollisionObservation> onExit)
           ObserveCollisionsToggled(IObservable<ICollisionObservation> onEnter, IObservable<ICollisionObservation> onExit, Component disposalTarget)
        {
            var onEnterSubject = new Subject<ICollisionObservation>();
            var onExitSubject = new Subject<ICollisionObservation>();

            var collidersEntered = new HashSet<Collider>();

            onEnter
                .Where(result => !collidersEntered.Contains(result.CollidingOther))
                .Subscribe(result =>
                {
                    collidersEntered.Add(result.CollidingOther);
                    onEnterSubject.OnNext(result);
                })
                .AddTo(disposalTarget);

            onExit
                .Where(result => collidersEntered.Contains(result.CollidingOther))
                .Subscribe(result =>
                {
                    collidersEntered.Remove(result.CollidingOther);
                    onExitSubject.OnNext(result);
                })
                .AddTo(disposalTarget);

            return (onEnterSubject, onExitSubject);
        }
    }
}

