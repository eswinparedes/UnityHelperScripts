using SUHScripts.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public interface ICollisionObservation
    {
        Option<Collision> CollisionData { get; }
        Collider CollidingOther { get; }
    }

    public class CollisionObserved : ICollisionObservation
    {
        public CollisionObserved(Option<Collision> collisionData, Collider collidingOther)
        {
            CollisionData = collisionData;
            CollidingOther = collidingOther;
        }

        public Option<Collision> CollisionData { get; }

        public Collider CollidingOther { get;}
    }
    public class CollisionObservation : ICollisionObservation
    {
        public CollisionObservation(Func<Option<Collision>> collisionDataSource, Func<Collider> collidingOtherSource)
        {
            m_collisionDataSource = collisionDataSource;
            m_collidingOtherSource = collidingOtherSource;
        }

        Func<Option<Collision>> m_collisionDataSource;
        Func<Collider> m_collidingOtherSource;
        public Option<Collision> CollisionData => m_collisionDataSource();
        public Collider CollidingOther => m_collidingOtherSource();
    }

    public class CollisionObservationMutable : ICollisionObservation
    {
        public Option<Collision> CollisionData { get; set; }
        public Collider CollidingOther { get; set; }
    }
}

