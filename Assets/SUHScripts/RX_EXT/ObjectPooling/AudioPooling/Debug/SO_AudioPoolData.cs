using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "SUHS/Audio/Audio Pool Data")]
    public class SO_AudioPoolData : AAudioPoolData
    {
        [Header("Loop defaults to min settings")]
        [SerializeField] Vector2 m_volumeRange = new Vector2(1, 1);
        [SerializeField] Vector2 m_pitchrange = new Vector2(1, 1);
        [SerializeField] Vector2 m_spatialBlendRange = new Vector2(0, 0);
        [SerializeField] List<AudioClip> m_clips = default;

        public override AudioClip Clip =>
            m_clips.RandomElement();

        public override PoolableAudioSettings Settings
        {
            get
            {
                var vol = Random.Range(m_volumeRange.x, m_volumeRange.y);
                var pitch = Random.Range(m_pitchrange.x, m_pitchrange.y);
                var blend = Random.Range(m_spatialBlendRange.x, m_spatialBlendRange.y);
                return new PoolableAudioSettings(vol, pitch,  blend);
            }
        }
    }
}

