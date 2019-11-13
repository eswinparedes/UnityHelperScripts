using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PoolableAudioSettings
{
    public float volume;
    public float pitch;
    public float spatialBlend;

    public PoolableAudioSettings(float volume, float pitch)
    {
        this.volume = volume;
        this.pitch = pitch;
        this.spatialBlend = 0;
    }

    public PoolableAudioSettings(float volume, float pitch, float spatialBlend)
    {
        this.volume = volume;
        this.pitch = pitch;
        this.spatialBlend = spatialBlend;
    }
}
