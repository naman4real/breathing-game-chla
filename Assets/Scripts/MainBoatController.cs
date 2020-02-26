using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoatController : MonoBehaviour
{
    private float speed = 20f;
    private float speedMultiplier = 2.5f;
    private bool breathIsOn = false;
    private bool collision = false;
    private Renderer gameBoat;
    private Rigidbody playerBody;
    // Create GameObject to find OSC
    private GameObject OSC;
    // Hold OSC data in spirometer object
    private OSC spirometer;

    // Start is called before the first frame update
    void Start()
    {
        OSC = findGameObject("OSC");
        spirometer = OSC.GetComponent<OSC>();
        spirometer.SetAddressHandler("/m5Analog", ReceiveSpirometerData);
        playerBody = GetComponent<Rigidbody>();
        gameBoat = GetComponent<Renderer>();
        gameBoat.enabled = true;
    }

    // Update is called once per frame.
    void Update()
    {
        // If arrow is down, turn on breath.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            breathIsOn = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            breathIsOn = false;
        }
    }

    private GameObject findGameObject(string name)
    {
        return GameObject.Find(name);
    }

    // Place general movement in FixedUpdate to avoid shaking.
    private void FixedUpdate()
    {
        // Move the boat forward at constant pace.
        if (!breathIsOn)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        // Accelerate boat.
        if (breathIsOn)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed * speedMultiplier);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        collision = true;
        // If the boat collides with an object, blink on and off. 
        StartCoroutine(BlinkTime(2f));
    }

    private void ReceiveSpirometerData(OscMessage message)
    {
        float breathVal = message.GetFloat(0);
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
            timeCounter += (1f/3f);
        }
        gameBoat.enabled = true;
        collision = false;
    }
}


