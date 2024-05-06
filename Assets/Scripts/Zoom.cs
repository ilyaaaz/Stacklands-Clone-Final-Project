using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] Camera zoomcam;
    float zoomSpeed = 8f;
    float minZoom = 3f;
    float maxZoom = 12f;
    [HideInInspector] public bool enableDrag;
    bool drag = false;
    Vector3 offSet, origin;

    private void Start()
    {
        enableDrag = true;
    }
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

        if (enableDrag)
        {
            if (Input.GetMouseButton(0))
            {
                offSet = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;
                if (!drag)
                {
                    drag = true;
                    origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                drag = false;
            }

            if (drag)
            {
                Camera.main.transform.position = origin - offSet;
            }
        } 
    }
}