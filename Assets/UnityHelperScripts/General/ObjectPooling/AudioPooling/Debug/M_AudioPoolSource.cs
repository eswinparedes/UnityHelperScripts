using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AudioPoolSource : MonoBehaviour
{
    public void Play(SO_AudioPoolData data)
    {
        AudioPool.PlaySoundOneShot(data.Clip, data.Settings);
    }

    public void PlayAt(SO_AudioPoolData data, Transform location)
    {
        AudioPool.PlaySoundOneShot(data, location);
    }
}
