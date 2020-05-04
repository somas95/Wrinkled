using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    public Camera mainCamera;
    public float horizontalSpeed;
    public float verticalSpeed;

    float h, v;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        h = horizontalSpeed * Input.GetAxis("Mouse X");
        v = verticalSpeed * Input.GetAxis("Mouse Y");

        transform.Rotate(0,h,0);
        mainCamera.transform.Rotate(v, 0, 0);
        
    }
}
