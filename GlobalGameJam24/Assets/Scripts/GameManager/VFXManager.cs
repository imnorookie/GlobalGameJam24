using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{

    public static VFXManager _instance;

    [SerializeField]
    private GameObject m_stunPrefab;

    public void Start() {
        _instance = this;
    }


    public GameObject PlayStunVFXAtPos(Transform pos) {
        return Instantiate(m_stunPrefab, pos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
