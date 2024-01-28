using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{

    public static VFXManager _instance;

    [SerializeField]
    private GameObject m_stunPrefab;

    [SerializeField]
    private GameObject m_waterSplashPrefab;

    [SerializeField]
    private float m_waterSplashTime = 0.5f;

    [SerializeField]
    private int m_stunFreezeFrames = 1;

    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float zoomLvl = 1.0f;

    [SerializeField]
    private float zoomTime = 0.25f;

    public void Awake() {
        _instance = this;
    }

    private IEnumerator StunFreezeFrameVFX(Vector3 position) {
        CameraZoom camera = GetComponent<CameraZoom>();
        camera.PanAndZoom(position, zoomLvl, zoomTime);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(m_stunFreezeFrames * 0.016f);
        Time.timeScale = 1.0f;
        camera.PanAndZoomToOriginalLocation(zoomTime);
    }

    public GameObject PlayStunVFXAtPos(Transform pos) {
        StartCoroutine(StunFreezeFrameVFX(pos.position));
        return Instantiate(m_stunPrefab, pos);
    }

    public void PlayWaterSplashVFXAtPos(Transform pos) {
        PlayVFXAtPosForTime(m_waterSplashPrefab, pos, m_waterSplashTime);
    }

    public void PlayVFXAtPosForTime(GameObject VFX, Transform pos, float time) {
        GameObject instantiatedVFX = Instantiate(VFX, pos.position, pos.rotation);
        StartCoroutine(DestroyVFXAfterTime(instantiatedVFX, time));
    }

    private IEnumerator DestroyVFXAfterTime(GameObject VFX, float time) {
		yield return new WaitForSeconds(time);
		Destroy(VFX);
	}


    // Update is called once per frame
    void Update()
    {
        
    }
}
