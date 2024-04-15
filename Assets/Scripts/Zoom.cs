using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] Camera zoomcam;
    float zoomSpeed = 8f;
    float minZoom = 3f;
    float maxZoom = 12f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (zoomcam.orthographic)
        {
            zoomcam.orthographicSize -= scroll;
            zoomcam.orthographicSize = Mathf.Clamp(zoomcam.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            zoomcam.fieldOfView -= scroll;
            zoomcam.fieldOfView = Mathf.Clamp(zoomcam.fieldOfView, minZoom, maxZoom);
        }
    }
}