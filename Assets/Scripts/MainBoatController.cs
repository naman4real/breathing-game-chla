using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoatController : MonoBehaviour
{
    private float speed = 20f;
    private float speedMultiplier = 2.5f;
    private bool breathIsOn = false;
    private float horizontalInput;
    private float turnSpeed = 80f;
    private bool collision = false;
    private Rigidbody playerBody;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame.
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
        // If arrow is down, turn on accelerator.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            breathIsOn = true;
            //playerBody.AddForce(-transform.right * speed * speedMultiplier, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            breathIsOn = false;
        }
    }

    // Place general movement in FixedUpdate to avoid shaking.
    private void FixedUpdate()
    {
        // Move the boat forward at constant pace.
        if (!breathIsOn && !collision)
        {
            //transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        // Accelerate boat.
        if (breathIsOn && !collision)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed * speedMultiplier);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerBody.useGravity = true;
        collision = true;
    }

}