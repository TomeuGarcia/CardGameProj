using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Pass Button")]
    [SerializeField] private AudioClip passButtonPressed_ac;

    [Header("Score")]
    [SerializeField] private AudioClip scoreUp_ac;


    private void OnEnable()
    {
        PassButton.OnPassPlayAudio += PlayPassButtonPressed;
        BoardSide.OnScoreUp += PlayScoreUp;
    }

    private void OnDisable()
    {
        PassButton.OnPassPlayAudio -= PlayPassButtonPressed;
        BoardSide.OnScoreUp -= PlayScoreUp;
    }




    private void PlayPassButtonPressed()
    {
        return;
        sfxAudioSource.volume = 0.5f;
        sfxAudioSource.clip = passButtonPressed_ac;
        sfxAudioSource.pitch = Random.Range(0.9f, 1.1f);
        sfxAudioSource.Play();
    }

    private void PlayScoreUp(float pitch)
    {
        return;
        sfxAudioSource.volume = 0.1f;
        sfxAudioSource.clip = scoreUp_ac;
        sfxAudioSource.pitch = pitch;
        sfxAudioSource.Play();
    }


}
