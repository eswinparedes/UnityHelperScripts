using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolableAudioExtensions
{
    public static PoolableAudioSettings QuickSettings =
        new PoolableAudioSettings()
        {
            volume = 1,
            pitch = 1,
            loop = false
        };

    public static void PlaySoundOneShot(this PoolableAudio @this, AudioClip clip, PoolableAudioSettings settings)
    {
        @this.Source.volume = settings.volume;
        @this.Source.pitch = settings.pitch;
        @this.Source.PlayOneShot(clip);
    }

    public static void PlaySoundOneShot(this PoolableAudio @this, AudioClip clip)
    {
        @this.PlaySoundOneShot(clip, QuickSettings);
    }
}
