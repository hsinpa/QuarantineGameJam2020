using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private float elapsedTime = 0;

    public AudioSource _audioSource;

    public AudioClip _researchCompleteAudio;
    public AudioClip _buttonClickAudio;
    public AudioClip _uiCloseAudio;
    public AudioClip[] _coughAudios;

    public void OnButtonClick()
    {
        _audioSource.PlayOneShot(_buttonClickAudio);
    }

    public void OnUIClose()
    {
        _audioSource.PlayOneShot(_uiCloseAudio);
    }

    public void OnResearchComplete()
    {
        _audioSource.PlayOneShot(_researchCompleteAudio);
    }

    public void PlayRandomCough()
    {
        int idx = UnityEngine.Random.Range(0, _coughAudios.Length - 1);

        _audioSource.PlayOneShot(_coughAudios[idx], 0.2f);
    }

    private void Awake()
    {
        elapsedTime = 0;
        _audioSource = this.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > 5.0f)
        {
            elapsedTime = 0;
            PlayRandomCough();
        }
    }

}
