using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoatController : MonoBehaviour
{
    public bool exhalePhase = false;
    public bool inhalePhase = true;
    public float exhaleTargetTime;
    public float inhaleTargetTime;
    public float exhaleDuration = 0f;
    public float inhaleDuration = 0f;
    public float cycles;

    public AudioClip coin;
    public AudioClip crash;
    public AudioClip treasure;
    
    private float downTime = 0f;
    private float upTime = 0f;
    private float exhaleStart = 0f;
    private float inhaleStart = 0f;
    //private bool exhaleIsDone = false;
    //private bool inhaleIsDone = false;
    public bool exhaleIsOn = true;
    public bool inhaleIsOn = false;

    private float exhaleThresh = 1480f;
    private float inhaleTresh = 1300f;
    private float steadyThresh = 1340f;

    private float speedMultiplier = 0.175f;
    //private float speed = 10f;

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
        //spirometer.SetAllMessageHandler(ReceiveSpirometerData);

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
        // Change boat direction based on camera in VR.
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);

        // Take cross product to ensure that boat goes forward.
        Vector3 cameraVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        Vector3 forwardDir = Vector3.Cross(cameraVector, new Vector3(0, 1, 0));
        Vector3 stationary = new Vector3(0, 0, 0);

        // Accelerate boat when player is exhaling.
        if (exhaleIsOn && exhalePhase)
        {
            if (cameraBounds())
            {
                inhaleDuration = 0;
                downTime = Time.time;
                boatBody.AddRelativeForce(new Vector3(forwardDir.x, 0, forwardDir.z) * speedMultiplier, ForceMode.VelocityChange);
                //transform.Translate(new Vector3(forwardDir.x, 0, forwardDir.z) * Time.deltaTime * speed);
                //boatBody.AddForce(new Vector3(forwardDir.x, 0, forwardDir.z), ForceMode.Impulse);
                exhaleDuration = downTime - exhaleStart;
            }
        }

        if (inhaleIsOn && inhalePhase)
        {
            if (cameraBounds())
            {
                exhaleDuration = 0;
                upTime = Time.time;
                inhaleDuration = upTime - inhaleStart;
            }
        }

        if (!exhaleIsOn && !inhaleIsOn)
        {
            exhaleStart = Time.time;
            inhaleStart = Time.time;
            var oppositeDir = -boatBody.velocity;
            boatBody.AddForce(oppositeDir);
            if (inhalePhase)
            {
                if (inhaleDuration > 1)
                {
                    inhalePhase = false;
                    exhalePhase = true;
                }
            }
            if(exhalePhase)
            {
                if (exhaleDuration > 1)
                {
                    inhalePhase = true;
                    exhalePhase = false;
                }
            }
            
        }
    }

    private void ReceiveSpirometerData(OscMessage message)
    {
        float breathVal = message.GetFloat(0);
        Debug.Log(breathVal);
        if (breathVal >= exhaleThresh)
        {
            audio.Play();
            exhaleIsOn = true;
            inhaleIsOn = false;
        }

        if (breathVal < exhaleThresh && breathVal > inhaleTresh )
        {
            audio.Stop();
            exhaleIsOn = false;
            inhaleIsOn = false;
        }

        if (breathVal <= inhaleTresh)
        {
            inhaleIsOn = true;
            exhaleIsOn = false;
        }
    }

    // Determine actions when boat collides with other gameObjects
    private void OnTriggerEnter(Collider other)
    {
        // If it collides with a coin.
        if (other.gameObject.CompareTag("Coin") || other.gameObject.CompareTag("Coin Two"))
        {
            Destroy(other.gameObject);
            audio.PlayOneShot(coin, 5f);
        }
        // If it collides with a treasure chest.
        else if (other.gameObject.CompareTag("Treasure"))
        {
            audio.PlayOneShot(treasure, 3f);
            Destroy(other.gameObject);
            Destroy(GameObject.FindGameObjectWithTag("Sparkle"));
        }
        else if(other.gameObject.CompareTag("Cliff"))
        {
            audio.PlayOneShot(crash, 5f);
            StartCoroutine(BlinkTime(2f));
            Vector3 previousPos = transform.position;
            transform.Translate(previousPos);
        }
        // If it collides with any other object.
        else
        {
            // If the boat collides with an object, blink on and off.
            audio.PlayOneShot(crash, 5f);
            StartCoroutine(BlinkTime(2f));
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

    // Only allow player to accelerate when looking in the forward direction.
    private bool cameraBounds()
    {
        if(Camera.main.transform.rotation.eulerAngles.y <= 45 && Camera.main.transform.rotation.eulerAngles.y >= -45)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


