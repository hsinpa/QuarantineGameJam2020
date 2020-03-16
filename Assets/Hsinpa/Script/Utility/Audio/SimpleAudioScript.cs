using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioScript : MonoBehaviour
{

    [SerializeField]
    AudioClip[] audioClips;

    [SerializeField]
    AudioSource audioSource;

    public static SimpleAudioScript instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioClip FindAudioClipByName(string p_name) {
        if (audioClips == null) return null;

        foreach (var clip in audioClips)
        {
            if (clip.name == p_name) return clip;
        }
        return null;
    }

    public void PlayAudio(string p_name)
    {
        var audioClip = FindAudioClipByName(p_name);

        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
