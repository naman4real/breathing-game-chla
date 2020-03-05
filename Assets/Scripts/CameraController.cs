using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool exhaleIsOn = false;
    public float exhaleTarget;
    public bool inhaleIsOn = false;
    public float inhaleTarget;

    //public AudioClip coin;
    //public AudioClip crash;
    //public AudioClip treasure;

    private float exhaleDuration = 0f;
    private float downTime = 0f;
    private float exhaleStart = 0f;
    private bool exhaleIsDone = false;

    private float speed = 20f;
    private float speedMultiplier = 4f;
    private float turnSpeed = 100f;
    private float horizontalInput;

    private GameObject wind;
    private AudioSource windAudio;

    // Create GameObject to find OSC
    private GameObject OSC;
    // Hold OSC data in spirometer object
    private OSC spirometer;

    // Start is called before the first frame update
    void Start()
    {
        OSC = GameObject.Find("OSC");
        spirometer = OSC.GetComponent<OSC>();
        spirometer.SetAddressHandler("/Spirometer/C", ReceiveSpirometerData);

        wind = GameObject.Find("Wind");
        windAudio = wind.GetComponent<AudioSource>();
    }

    // Update is called once per frame.
    void Update()
    {
        // If up arrow is down, turn on exhalation.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            windAudio.Play();
            exhaleIsOn = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            windAudio.Stop();
            exhaleIsOn = false;
        }
        // If space bar is down, turn on inhalation.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inhaleIsOn = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            inhaleIsOn = false;
        }

    }

    // Place general movement in FixedUpdate to avoid shaking.
    private void FixedUpdate()
    {
        //Move boat side to side. (FOR MOUSE PLAY)
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        // Move the boat forward at constant pace.
        if (!exhaleIsOn)
        {
            exhaleIsDone = false;
            exhaleStart = Time.time;
            transform.Translate(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * Time.deltaTime * speed);
        }
        // Accelerate boat.
        if (exhaleIsOn)
        {
            downTime = Time.time;
            transform.Translate(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * Time.deltaTime * speed * speedMultiplier);
            exhaleIsDone = true;
        }

        // Determine how long the breath was exhaled. PUT THRESHOLD CODE FOR EXHALE HERE--
        if (exhaleIsDone == true)
        {
            exhaleDuration = downTime - exhaleStart;
        }
    }

    private void ReceiveSpirometerData(OscMessage message)
    {
        float breathVal = message.GetFloat(0);
    }

}


