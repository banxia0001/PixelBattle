using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    public Camera cam;
    public float speedModi = 1;

    private Vector3 targetMove;
    // Start is called before the first frame update
    void Start()
    {
        targetMove = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            targetMove += transform.up * Time.deltaTime * 30f * speedModi;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            targetMove += transform.up * Time.deltaTime * -30f * speedModi;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            targetMove += transform.right * Time.deltaTime * -30f * speedModi;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            targetMove += transform.right * Time.deltaTime * 30f * speedModi;
        }

        this.transform.position = Vector3.Lerp(this.transform.position, targetMove, 0.9f);

        cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 30f;
        if (cam.orthographicSize > 15.5f) cam.orthographicSize = 15.5f;
        if (cam.orthographicSize < 5f) cam.orthographicSize = 5f;
    }
}
