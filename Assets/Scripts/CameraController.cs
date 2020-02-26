using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(13.69f, 19.96f, -0.32f);
    //private float horizontalInput;
    //private float turnSpeed = 25f;
    //private float windowLimit = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
        //horizontalInput += Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed;
        //horizontalInput = Mathf.Clamp(horizontalInput, -windowLimit, windowLimit);
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 270 + horizontalInput, transform.localEulerAngles.z);
        // Offset camera behind ship.

        //transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * turnSpeed * horizontalInput);

    }
}
