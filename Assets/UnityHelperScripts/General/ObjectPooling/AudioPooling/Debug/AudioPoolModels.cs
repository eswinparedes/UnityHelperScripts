using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public abstract class AAudioPoolData : ScriptableObject, IAudioPoolData
    {
        public abstract AudioClip Clip { get; }
        public abstract PoolableAudioSettings Settings { get; }
    }
    public interface IAudioPoolData
    {
        AudioClip Clip { get; }
        PoolableAudioSettings Settings { get; }
    }
}

