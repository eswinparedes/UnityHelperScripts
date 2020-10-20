using UnityEngine;

namespace SUHScripts
{
    public class AudioPoolPlayer : MonoBehaviour
    {
        [SerializeField] AudioSource m_source = default;


        void PlayQuick(AAudioPoolData data)
        {
            m_source.Stop();
            m_source.volume = data.Settings.volume;
            m_source.pitch = data.Settings.pitch;
            m_source.spatialBlend = data.Settings.spatialBlend;

            m_source.PlayOneShot(data.Clip);
        }
        public void PlayOnAudioSource(AAudioPoolData data)
        {
            m_source.loop = false;
            PlayQuick(data);
        }
        public void PlayOnAudioSourceLooped(AAudioPoolData data)
        {
            m_source.loop = true;

            PlayQuick(data);
        }
        public void Play(AAudioPoolData data)
        {
            AudioPool.PlaySoundOneShot(data.Clip, data.Settings);
        }

        public void PlayAt(AAudioPoolData data, Transform location)
        {
            AudioPool.PlaySoundOneShot(data, location);
        }
    }
}

