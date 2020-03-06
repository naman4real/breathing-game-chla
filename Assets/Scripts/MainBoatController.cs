using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoatController : MonoBehaviour
{
    public bool exhaleIsOn = false;
    public float exhaleTarget;
    public bool inhaleIsOn = false;
    public float inhaleTarget;

    public AudioClip coin;
    public AudioClip crash;
    public AudioClip treasure;

    private float exhaleDuration = 0f;
    private float downTime = 0f;
    private float exhaleStart = 0f;
    private bool exhaleIsDone = false;

    private float speed = 10f;
    private float speedMultiplier = 3f;
    private float turnSpeed = 100f;
    private float horizontalInput;

    private AudioSource audio;
    private Renderer gameBoat;

    // Create GameObject to find OSC
    private GameObject OSC;
    // Hold OSC data in spirometer object
    private OSC spirometer;
    // Get the boat as a rigidbody
    private Rigidbody boatBody;

    // Start is called before the first frame update
    void Start()
    {
        OSC = GameObject.Find("OSC");
        spirometer = OSC.GetComponent<OSC>();
        spirometer.SetAddressHandler("/Spirometer/C", ReceiveSpirometerData);
        gameBoat = GetComponent<Renderer>();
        gameBoat.enabled = true;
        boatBody = GetComponent<Rigidbody>();

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame.
    void Update(){}

    // Place general movement in FixedUpdate to avoid shaking.
    private void FixedUpdate()
    {
        // Move boat side to side. (FOR MOUSE PLAY)
        // horizontalInput = Input.GetAxis("Horizontal");
        // transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        // Change boat direction based on camera in VR.
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);

        // Take cross product to ensure that boat goes forward.
        Vector3 cameraVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        Vector3 forwardDir = Vector3.Cross(cameraVector, new Vector3(0, 1, 0));
        Vector3 stationary = new Vector3(0, 0, 0);

        // Accelerate boat when player is exhaling.
        if (exhaleIsOn)
        {
            downTime = Time.time;
            boatBody.AddRelativeForce(new Vector3(forwardDir.x, 0, forwardDir.z), ForceMode.VelocityChange);
            // boatBody.AddForce(new Vector3(forwardDir.x, 0, forwardDir.z), ForceMode.Impulse);
            // transform.Translate(new Vector3(forwardDir.x, 0, forwardDir.z) * Time.deltaTime * speed * speedMultiplier);
            exhaleIsDone = true;
        }

        if (!exhaleIsOn && !inhaleIsOn) 
        {
            var oppositeDir = -boatBody.velocity;
            boatBody.AddForce(oppositeDir);
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

        if (breathVal >= 1400)
        {
            audio.Play();
            exhaleIsOn = true;
        }

        if (breathVal < 1400 && breathVal > 1100 )
        {
            audio.Stop();
            exhaleIsOn = false;
            inhaleIsOn = false;
        }

        if (breathVal <= 1100)
        {
            inhaleIsOn = true;
        }
    }

    // Determine actions when boat collides with other gameObjects
    private void OnTriggerEnter(Collider other)
    {
        // If it collides with a coin.
        if (other.gameObject.CompareTag("Coin"))
        {
            if (exhaleIsOn)
            {
                Destroy(other.gameObject);
                audio.PlayOneShot(coin, 5f);
            }
        }
        // If it collides with a treasure chest.
        else if (other.gameObject.CompareTag("Treasure"))
        {
            if (inhaleIsOn)
            {
                audio.PlayOneShot(treasure, 1f);
                Destroy(other.gameObject);
                Destroy(GameObject.Find("Sparkle"));
            }
        }
        // If it collides with any other object.
        else
        {
            if (!other.gameObject.CompareTag("Ocean"))
            {
                // If the boat collides with an object, blink on and off.
                audio.PlayOneShot(crash, 5f);
                StartCoroutine(BlinkTime(2f));
            }
        }
    }

    // Blink the boat on and off.
    private IEnumerator BlinkTime(float blinkDuration)
    {
        float timeCounter = 0;
        while (timeCounter < blinkDuration)
        {
            // make the boat blink off and on.
            gameBoat.enabled = !gameBoat.enabled;
            //wait 1 second per interval
            yield return new WaitForSeconds(0.3f);
            timeCounter += (1f / 3f);
        }
        gameBoat.enabled = true;
    }
}


