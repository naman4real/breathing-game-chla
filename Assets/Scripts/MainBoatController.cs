using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoatController : MonoBehaviour
{
    private float speed = 20f;
    private float speedMultiplier = 2.5f;
    private float appearTime = 1000f;
    private bool breathIsOn = false;
    private bool collision = false;
    public Renderer gameBoat;
    public float blinkInterval = 1f;
    public float blinkStartDelay = 0.0f;
    private Rigidbody playerBody;
    // Start is called before the first frame update
    void Start()
    {
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
        StartCoroutine(BlinkTime(2.5f));
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


