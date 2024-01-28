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
    private AudioClip m_oarCollisionAudio;

    [SerializeField]
    private AudioClip m_oarOnPlayerCollisionAudio;
    
    [SerializeField]
    private AudioClip[] m_oarOnPlayerHurtAudio;

    [SerializeField]
    private AudioClip m_fishOnPlayerCollisionAudio;

    [SerializeField]
    private AudioClip[] m_fishOnPlayerHurtAudio;

    [SerializeField]
    private AudioClip[] m_fishOnOarCollisionAudio;


    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }

    public void PlayBoatCollisionSFX() {
        AudioSource.PlayClipAtPoint(m_boatCollisionAudio, Vector3.zero);
    }

    public void PlayOarCollisionSFX() {
        AudioSource.PlayClipAtPoint(m_oarCollisionAudio, Vector3.zero);
    }


    private void playRandomAudio(AudioClip[] audioClips) {
        int randInt = Random.Range(0,audioClips.Length);
        AudioSource.PlayClipAtPoint(audioClips[randInt], Vector3.zero);

    }

    private void hurtPlayer(AudioClip hitAudio, AudioClip[] hurtAudio) {
        AudioSource.PlayClipAtPoint(hitAudio, Vector3.zero);
        playRandomAudio(hurtAudio);
    }


    public void PlayOarOnPlayerCollisionSFX() {
        hurtPlayer(m_oarOnPlayerCollisionAudio, m_oarOnPlayerHurtAudio);
    }

    public void PlayFishOnPlayerCollisionSFX() {
        hurtPlayer(m_fishOnPlayerCollisionAudio, m_fishOnPlayerHurtAudio);
    }

    public void PlayFishOnOarCollisionSFX() {
        playRandomAudio(m_fishOnOarCollisionAudio);
    }


}
