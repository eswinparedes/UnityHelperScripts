using UnityEngine;

namespace SUHScripts
{
    public class ResetPooledRigidBodyComponent : MonoBehaviour, IPoolableComponent {

        [SerializeField] Rigidbody _rBody = default;

        public void Spawned()
        {

        }
    
        public void Despawned()
        {
            _rBody.velocity = Vector3.zero;
            _rBody.angularVelocity = Vector3.zero;
        }
    }
}

