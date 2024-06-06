using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    [Header("Camera")]
    public Transform camFolder;
    public Camera cam;
    public float camSpeed = 1f;
    private float camZoom = 15f;
    private float camZoom_Now = 15f;


    private Vector3 followPos;

    private void Start()
    {
        followPos = this.transform.position;
    }
    private void Update()
    {
        camZoom -= Input.GetAxis("Mouse ScrollWheel") * 10f;
        if (camZoom >= 15f) camZoom = 15f;
        if (camZoom <= 5f) camZoom = 5f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) CamInMove(1);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) CamInMove(2);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) CamInMove(3);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) CamInMove(4);
    }
    private void LateUpdate()
    {
        CamUpdate();
    }
    private void CamUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, followPos, 0.12f);
        camZoom_Now = Mathf.MoveTowards(camZoom_Now, camZoom, Time.deltaTime * 30f);
        cam.orthographicSize = camZoom_Now;
    }
    private void CamInMove(int Dir)
    {
        if (Dir == 1)
        {
            if (followPos.z >= LandManager._landHeight) return;
            followPos += transform.forward * Time.deltaTime * camSpeed * camZoom;
        }

        if (Dir == 2)
        {
            if (followPos.z <= 0) return;
            followPos += transform.forward * Time.deltaTime * camSpeed * camZoom * -1;
        }

        if (Dir == 3)
        {
            if (followPos.x <= 0) return;
            followPos += transform.right * Time.deltaTime * camSpeed * camZoom * -1;
        }

        if (Dir == 4)
        {
            if (followPos.x >= LandManager._landLength) return;
            followPos += transform.right * Time.deltaTime * camSpeed * camZoom;
        }
    }
}
