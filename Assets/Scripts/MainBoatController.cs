using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoatController : MonoBehaviour
{

    public AudioClip coin;
    public AudioClip crash;
    public AudioClip treasure;

    public GameObject camera;

    private Renderer gameBoat;
    private AudioSource audio;

    private Vector3 offset;

    private CameraController cameraController;

    // Use this for initialization
    void Start()
    {
        //offset = transform.position - camera.transform.position;

        //gameBoat = GetComponent<Renderer>();
        //gameBoat.enabled = true;

        //distance = offset.magnitude;
        //playerPrevPos = camera.transform.position;
        //audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame.
    void LateUpdate()
    {
        transform.position = new Vector3(camera.transform.position.x, 1.04f, camera.transform.position.z);
        transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles.x, camera.transform.rotation.eulerAngles.y + 90, camera.transform.rotation.eulerAngles.z);
    }


    //// Determine actions when boat collides with other gameObjects
    //private void OnTriggerEnter(Collider other)
    //{
    //    // If it collides with a coin.
    //    if (other.gameObject.CompareTag("Coin"))
    //    {
    //        if (exhaleIsOn)
    //        {
    //            Destroy(other.gameObject);
    //            audio.PlayOneShot(coin, 5f);
    //        }
    //    }
    //    // If it collides with a treasure chest.
    //    else if (other.gameObject.CompareTag("Treasure"))
    //    {
    //        if (inhaleIsOn)
    //        {
    //            audio.PlayOneShot(treasure, 1f);
    //            Destroy(other.gameObject);
    //            Destroy(GameObject.Find("Sparkle"));
    //        }
    //    }
    //    // If it collides with any other object.
    //    else
    //    {
    //        if (!other.gameObject.CompareTag("Ocean"))
    //        {
    //            // If the boat collides with an object, blink on and off.
    //            audio.PlayOneShot(crash, 5f);
    //            StartCoroutine(BlinkTime(2f));
    //        }

    //    }
    //}


    ////Blink the boat on and off.
    //private IEnumerator BlinkTime(float blinkDuration)
    //{
    //    float timeCounter = 0;
    //    while (timeCounter < blinkDuration)
    //    {
    //        // make the boat blink off and on.
    //        gameBoat.enabled = !gameBoat.enabled;
    //        //wait 1 second per interval
    //        yield return new WaitForSeconds(0.3f);
    //        timeCounter += (1f / 3f);
    //    }
    //    gameBoat.enabled = true;
    //}
}