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

    public void Start() {
        _instance = this;
    }


    public GameObject PlayStunVFXAtPos(Transform pos) {
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
