using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera camera;
    private Vector2 camOrigPos;

    private float camOrigZoom;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camOrigPos = camera.transform.position;
        camOrigZoom = camera.orthographicSize;
    }

    public void PanAndZoom(Vector2 pos, float zoom, float time) {
        Debug.Log(pos);
        Debug.Log(camOrigPos);
        StartCoroutine(cameraLerpOverTime(pos, zoom, time));
    }

    public void PanAndZoomToOriginalLocation(float time) {
        PanAndZoom(camOrigPos, camOrigZoom, time);
    }
    
    private IEnumerator cameraLerpOverTime(Vector3 pos, float zoom, float time) {
        Vector3 curPos = camera.transform.position;
        Vector3 posCorrected = new Vector3(pos.x, pos.y, -10);
        float curZoom = camera.orthographicSize;
        float elapsed = 0.0f;
        while (elapsed <= time) {
            Debug.Log(curPos);
            float t = elapsed / time;

            curPos = Vector3.Lerp(curPos, posCorrected, t);
            curZoom = Mathf.Lerp(curZoom, zoom, t);

            camera.transform.position = curPos;
            camera.orthographicSize = curZoom;
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Done");
    }

}
