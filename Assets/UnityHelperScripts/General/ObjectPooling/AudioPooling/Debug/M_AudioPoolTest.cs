using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AudioPoolTest : MonoBehaviour
{
    [SerializeField] AudioClip spawnAudio = default;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AudioPool.PlaySoundOneShot(spawnAudio);
        }
    }
}
