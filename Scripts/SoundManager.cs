using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [Space]
    [SerializeField] private AudioClip _clickStarSfx;

    public static UnityEvent<AudioClip> OnPlaySFX = new();
    // Start is called before the first frame update
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        _audioSource.PlayOneShot(clip);
    }

   

    private void OnDestroy()
    {
        OnPlaySFX.RemoveAllListeners();
    }
}
