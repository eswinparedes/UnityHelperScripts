using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace SUHScripts
{
    using static PoolableAudioExtensions;
    public static class AudioPool 
    {
        public static HashSet<PoolableAudio> m_spawned = new HashSet<PoolableAudio>();
        public static GameObject Instance { get; private set; } = null;
        public static GameObject Prefab { get; private set; }

        public static IDisposable m_currentUpdateSubscription = null;
        public static void Reset()
        {
            m_spawned.Clear();
            m_currentUpdateSubscription?.Dispose();

            foreach (var a in m_spawned)
            {
                PrefabPoolingSystem.Despawn(a.gameObject);
            }

            
        }
        //SUHS TODO: Remove need for "Get Component" Possibly by casting ?
        public static void PlaySoundOneShot(AudioClip clip, PoolableAudioSettings settings, Vector3? location = null)
        {
            if(Instance == null)
            {
                Instance = new GameObject();
                Instance.gameObject.name = "No instance AudioPool found, defaulting to this";

                m_currentUpdateSubscription = Instance.FixedUpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        m_spawned.RemoveWhere(p =>
                        {
                            bool shouldDespawn = !p.Source.isPlaying;
                            if (shouldDespawn) PrefabPoolingSystem.Despawn(p.gameObject);

                            return shouldDespawn;
                        });
                    })
                    .AddTo(Instance);


                Prefab = (GameObject) Resources.Load("SUHScripts/General/ObjectPooling/QuickPoolableAudio Variant");
            }

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

        public static void PlaySoundOneShot(IAudioPoolData data)
        {
            PlaySoundOneShot(data.Clip, data.Settings);
        }

        public static void PlaySoundOneShot(AudioClip clip, Transform location)
        {
            PlaySoundOneShot(clip, QuickSettings, location.position);
        }

        public static void PlaySoundOneShot(IAudioPoolData data, Transform location)
        {
            PlaySoundOneShot(data.Clip, data.Settings, location.position);
        }

        public static void PlaySoundOneShot(AudioClip clip, Vector3 location)
        {
            PlaySoundOneShot(clip, QuickSettings, location);
        }

        public static void PlaySoundOneShot(IAudioPoolData data, Vector3 location)
        {
            PlaySoundOneShot(data.Clip, data.Settings, location);
        }



        public static void PlaySoundOnShot(IReadOnlyList<AudioClip> clips)
        {
            for (int i = 0; i < clips.Count; i++)
            {
                PlaySoundOneShot(clips[i], QuickSettings);
            }
        }

        public static void PlaySoundOneShot(IReadOnlyList<IAudioPoolData> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                PlaySoundOneShot(data[i].Clip, data[i].Settings);
            }
        }

        public static void PlaySoundOneShot(IReadOnlyList<AudioClip> clips, Transform location)
        {
            for (int i = 0; i < clips.Count; i++)
            {
                PlaySoundOneShot(clips[i], QuickSettings, location.position);
            }
        }

        public static void PlaySoundOneShot(IReadOnlyList<IAudioPoolData> data, Transform location)
        {
            for (int i = 0; i < data.Count; i++)
            {
                PlaySoundOneShot(data[i].Clip, data[i].Settings, location.position);
            }
        }

        public static void PlaySoundOneShot(IReadOnlyList<AudioClip> clips, Vector3 location)
        {
            for (int i = 0; i < clips.Count; i++)
            {
                PlaySoundOneShot(clips[i], QuickSettings, location);
            }
        }

        public static void PlaySoundOneShot(IReadOnlyList<IAudioPoolData> data, Vector3 location)
        {
            for (int i = 0; i < data.Count; i++)
            {
                PlaySoundOneShot(data[i].Clip, data[i].Settings, location);
            }
        }
    }
}

