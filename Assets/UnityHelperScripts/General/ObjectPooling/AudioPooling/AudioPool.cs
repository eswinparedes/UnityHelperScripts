using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static PoolableAudioExtensions;

public class AudioPool : A_Component
{
    [SerializeField] PoolableAudio prefab = default;

    public static HashSet<PoolableAudio> m_spawned = new HashSet<PoolableAudio>();
    public static AudioPool Instance { get; private set; } = null;
    public static GameObject Prefab { get; private set; }

    public static void PlaySoundOneShot(AudioClip clip, PoolableAudioSettings settings)
    {
        PoolableAudio poolableAudio = PrefabPoolingSystem.Spawn(Prefab.gameObject).GetComponent<PoolableAudio>();
        m_spawned.Add(poolableAudio);
        poolableAudio.PlaySoundOneShot(clip, settings);
    }

    public static void PlaySoundOneShot(AudioClip clip)
    {
        PlaySoundOneShot(clip, QuickSettings);
    }

    public override void Execute()
    {
        m_spawned.RemoveWhere(poolableAudio =>
        {
            bool shouldDespawn = !poolableAudio.Source.isPlaying;

            if (shouldDespawn)
                PrefabPoolingSystem.Despawn(poolableAudio.gameObject);

            return shouldDespawn;
        });
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
    }


}
