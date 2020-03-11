using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoatController : MonoBehaviour
{
    public bool exhalePhase = false;
    public bool inhalePhase = true;
    public float exhaleTargetTime = 1f;
    public float inhaleTargetTime = 1f;
    public float exhaleDuration;
    public float inhaleDuration;
    public float cycles = 5f;
    public float cycleCounter = 0f;
    public bool gameOver = false;
    public float speed;

    public AudioClip coin;
    public AudioClip crash;
    public AudioClip treasure;
    
    private float downTime = 0f;
    private float upTime = 0f;
    private float exhaleStart = 0f;
    private float inhaleStart = 0f;
    //private bool exhaleIsDone = false;
    //private bool inhaleIsDone = false;
    public bool exhaleIsOn = false;
    public bool inhaleIsOn = false;

    private float exhaleThresh = 1500f;
    private float inhaleTresh = 1100f;
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

    private ScoreBoard treasureScores;
    private ScoreBoard coinScores;
    private ScoreBoard finalScores;
    private ScoreBoard spedometer; 
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

        treasureScores = GameObject.FindGameObjectWithTag("Treasure Score").GetComponent<ScoreBoard>();
        coinScores = GameObject.FindGameObjectWithTag("Coin Score").GetComponent<ScoreBoard>();
        finalScores = GameObject.FindGameObjectWithTag("Final Score").GetComponent<ScoreBoard>();
        spedometer = GameObject.FindGameObjectWithTag("Final Score").GetComponent<ScoreBoard>();

    // Manually set inhale phase to true at start of game.
    inhalePhase = true;
    }

    // Update is called once per frame.
    void Update()
    {
        if (exhaleIsOn && exhalePhase)
        {
            if (cameraBounds())
            {
                audio.Play();
            }
        }
        if (!exhaleIsOn && !inhaleIsOn)
        {
            audio.Stop();
        }
    }

    // Place general movement in FixedUpdate to avoid shaking.
    private void FixedUpdate()
    {
        if(cycleCounter > cycles)
        {
            gameOver = true;
            Destroy(GameObject.FindGameObjectWithTag("Treasure"));
        }
        if (!gameOver)
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
                    audio.Play();
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
                if (exhalePhase)
                {
                    if (exhaleDuration > 1)
                    {
                        inhalePhase = true;
                        exhalePhase = false;
                    }
                }

            }
        }
    }

    private void ReceiveSpirometerData(OscMessage message)
    {
        float breathVal = message.GetFloat(0);
        speed = breathVal;
        Debug.Log(breathVal);
        if (breathVal >= exhaleThresh)
        {
            exhaleIsOn = true;
            inhaleIsOn = false;
        }

        if (breathVal < exhaleThresh && breathVal > inhaleTresh)
        {
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
            if (exhalePhase)
            {
                Destroy(other.gameObject);
                audio.PlayOneShot(coin, 5f);
                // Update all instances of coinScore so there is data consistency
                coinScores.coinScore += 1;
                treasureScores.coinScore += 1;
                finalScores.coinScore += 1;
                spedometer.coinScore += 1;
            }
        }
        // If it collides with a treasure chest.
        else if (other.gameObject.CompareTag("Treasure"))
        {
            if (inhalePhase)
            {
                audio.PlayOneShot(treasure, 3f);
                // Update all instances of treasureScore so there is data consistency
                coinScores.treasureScore += 1;
                treasureScores.treasureScore += 1;
                finalScores.treasureScore += 1;
                spedometer.treasureScore += 1;
                Destroy(other.gameObject);
            }
        }
        else if(other.gameObject.CompareTag("Cliff"))
        {
            audio.PlayOneShot(crash, 5f);
            StartCoroutine(BlinkTime(2f));
            if (transform.position.x >= 44)
            {
                transform.Translate(new Vector3(42, transform.position.y, transform.position.z));
            }
            if (transform.position.x <= -44)
            {
                transform.Translate(new Vector3(-42, transform.position.y, transform.position.z));
            }
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
        if(Camera.main.transform.rotation.eulerAngles.y <= 90 && Camera.main.transform.rotation.eulerAngles.y >= -90)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


