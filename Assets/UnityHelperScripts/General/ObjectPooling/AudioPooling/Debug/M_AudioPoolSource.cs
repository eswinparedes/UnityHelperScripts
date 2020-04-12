using UnityEngine;

namespace SUHScripts
{
    public class M_AudioPoolSource : MonoBehaviour
    {
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

