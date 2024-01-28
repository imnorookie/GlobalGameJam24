using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;

    [SerializeField]
    private AudioClip m_boatCollisionAudio;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_boatCollisionVol = 1.0f;

    [SerializeField]
    private AudioClip m_oarCollisionAudio;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_oarCollisionVol = 1.0f;


    [SerializeField]
    private AudioClip m_oarOnPlayerCollisionAudio;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_oarOnPlayerCollisionVol = 1.0f;
    
    [SerializeField]
    private AudioClip[] m_oarOnPlayerHurtAudio;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_oarOnPlayerHurtVol = 1.0f;


    [SerializeField]
    private AudioClip m_fishOnPlayerCollisionAudio;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_fishOnPlayerCollisionVol = 1.0f;


    [SerializeField]
    private AudioClip[] m_fishOnPlayerHurtAudio;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_fishOnPlayerHurtVol = 1.0f;


    [SerializeField]
    private AudioClip[] m_fishOnOarCollisionAudio;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_fishOnOarCollisionVol = 1.0f;


    [SerializeField]
    private AudioClip m_oarEnterWaterAudio;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_oarEnterWaterVol = 1.0f;

    [SerializeField]
    private AudioClip m_oarExitWaterAudio;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_oarExitWaterVol = 1.0f;


    [SerializeField]
    private AudioClip m_bgAmbience;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_bgAmbienceVol = 1.0f;


    [SerializeField]
    private AudioClip m_bgMusic;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_bgMusicVol = 1.0f;




    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

    public void PlayBoatCollisionSFX() {
        AudioSource.PlayClipAtPoint(m_boatCollisionAudio, Vector3.zero, m_boatCollisionVol);
    }

    public void PlayOarCollisionSFX() {
        AudioSource.PlayClipAtPoint(m_oarCollisionAudio, Vector3.zero, m_oarCollisionVol);
    }


    private void playRandomAudio(AudioClip[] audioClips, float vol) {
        int randInt = Random.Range(0,audioClips.Length);
        AudioSource.PlayClipAtPoint(audioClips[randInt], Vector3.zero, vol);

    }

    private void hurtPlayer(AudioClip hitAudio, AudioClip[] hurtAudio, float hitVol, float hurtVol) {
        AudioSource.PlayClipAtPoint(hitAudio, Vector3.zero, hitVol);
        playRandomAudio(hurtAudio, hurtVol);
    }


    public void PlayOarOnPlayerCollisionSFX() {
        hurtPlayer(m_oarOnPlayerCollisionAudio, m_oarOnPlayerHurtAudio,
            m_oarOnPlayerCollisionVol, m_oarOnPlayerHurtVol);
    }

    public void PlayFishOnPlayerCollisionSFX() {
        hurtPlayer(m_fishOnPlayerCollisionAudio, m_fishOnPlayerHurtAudio,
            m_fishOnOarCollisionVol, m_fishOnPlayerHurtVol);
    }

    public void PlayFishOnOarCollisionSFX() {
        playRandomAudio(m_fishOnOarCollisionAudio, m_fishOnOarCollisionVol);
    }

    public void PlayOarEnterWaterSFX() {
        AudioSource.PlayClipAtPoint(m_oarEnterWaterAudio, Vector3.zero, m_oarEnterWaterVol);
    }

    public void PlayOarExitWaterSFX() {
        AudioSource.PlayClipAtPoint(m_oarExitWaterAudio, Vector3.zero, m_oarExitWaterVol);
    }

    public AudioSource PlayLoopingAudio(AudioClip clip, float vol) {
        AudioSource audioSource = new GameObject(clip.name).AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.volume = vol;
        audioSource.Play();
        return audioSource;
    }

    public void PlayBGAmbience() {
        PlayLoopingAudio(m_bgAmbience, m_bgAmbienceVol);
    }

    public AudioSource PlayBGM() {
        return PlayLoopingAudio(m_bgMusic, m_bgMusicVol);
    }

    
}
