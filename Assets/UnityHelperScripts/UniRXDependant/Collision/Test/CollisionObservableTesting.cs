using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COps = SUHScripts.CollisionObservableEffectOperations;
using UniRx;

namespace SUHScripts.Tests
{
    public class CollisionObservableTesting : MonoBehaviour
    {
        [Header("Normal Observing")]
        [SerializeField] Collider m_singleCollisionCollider = default;
        [Header("Grouped Observing")]
        [SerializeField] List<Collider> m_groupedCollisionColliders = default;

        public void Awake()
        {
            var obvsSingle = COps.ObserveCollisions(m_singleCollisionCollider);
            var obvsGrouped = COps.ObserveCollisionsGrouped(m_groupedCollisionColliders, this);
            var obvsToggled = COps.ObserveCollisionsToggled(obvsSingle.onEnter, obvsGrouped.onEnter, this);

            return;
            obvsSingle
                .onEnter
                .Subscribe(_ => Debug.Log("Single Entered"))
                .AddTo(this);

            obvsSingle
                .onExit
                .Subscribe(_ => Debug.Log("Single Exited"))
                .AddTo(this);

            obvsGrouped
                .onEnter
                .Subscribe(_ => Debug.Log("Grouped Entered"))
                .AddTo(this);

            obvsGrouped
                .onExit
                .Subscribe(_ => Debug.Log("Grouped Exited"))
                .AddTo(this);

            obvsToggled
                .onEnter
                .Subscribe(_ => Debug.Log("Toggle Entered"))
                .AddTo(this);

            obvsToggled
                .onExit
                .Subscribe(_ => Debug.Log("Toggle Exited"))
                .AddTo(this);

        }
    }
}

