using UnityEngine;
using static PoolableAudioExtensions;

public class PoolableAudio : MonoBehaviour
{
    [SerializeField] AudioSource m_audioSource = default;

    public AudioSource Source => m_audioSource;
}
