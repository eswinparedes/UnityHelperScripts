using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts.Tests
{
    public class M_TestPrefabPoolableComponent : MonoBehaviour, IPoolableComponent
    {
        [SerializeField] string m_text = "";

        public void Despawned()
        {
            Debug.Log($"Despawned on {gameObject.name} with message {m_text}");
        }

        public void Spawned()
        {
            Debug.Log($"Spawned on {gameObject.name} with message {m_text}");
        }


    }
}

