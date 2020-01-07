using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace SUHScripts
{
    using static PoolableAudioExtensions;
    public class AudioPool : MonoBehaviour
    {
        [SerializeField] PoolableAudio prefab = default;

        public static HashSet<PoolableAudio> m_spawned = new HashSet<PoolableAudio>();
        public static AudioPool Instance { get; private set; } = null;
        public static GameObject Prefab { get; private set; }

        //SUHS TODO: Remove need for "Get Component" Possibly by casting ?
        public static void PlaySoundOneShot(AudioClip clip, PoolableAudioSettings settings, Vector3? location = null)
        {
            var audioGO = PrefabPoolingSystem.Spawn(Prefab.gameObject);
            var poolableAudio = audioGO.GetComponent<PoolableAudio>();

            if (location.HasValue)
                audioGO.transform.position = location.Value;

            m_spawned.Add(poolableAudio);
            poolableAudio.PlaySoundOneShot(clip, settings);
        }

        public static void PlaySoundOneShot(AudioClip clip)
        {
            PlaySoundOneShot(clip, QuickSettings);
        }

        public static void PlaySoundOneShot(SO_AudioPoolData data)
        {
            PlaySoundOneShot(data.Clip, data.Settings);
        }

        public static void PlaySoundOneShot(AudioClip clip, Transform location)
        {
            PlaySoundOneShot(clip, QuickSettings, location.position);
        }

        public static void PlaySoundOneShot(SO_AudioPoolData data, Transform location)
        {
            PlaySoundOneShot(data.Clip, data.Settings, location.position);
        }

        public static void PlaySoundOneShot(AudioClip clip, Vector3 location)
        {
            PlaySoundOneShot(clip, QuickSettings, location);
        }

        public static void PlaySoundOneShot(SO_AudioPoolData data, Vector3 location)
        {
            PlaySoundOneShot(data.Clip, data.Settings, location);
        }

        private void Start()
        {
            if(Instance != null)
            {
                Debug.LogError("AUDIO POOL INSTANCE ALREADY SET");
            }
            else
            {
                Instance = this;
                Prefab = prefab.gameObject;
            }

            M_UpdateManager
                .OnFixedUpdate_0
                .Subscribe(_ =>
                {
                    m_spawned.RemoveWhere(poolableAudio =>
                    {
                        bool shouldDespawn = !poolableAudio.Source.isPlaying;

                        if (shouldDespawn)
                            PrefabPoolingSystem.Despawn(poolableAudio.gameObject);

                        return shouldDespawn;
                    });
                })
                .AddTo(this);
        }
    }
}

