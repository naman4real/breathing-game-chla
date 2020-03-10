using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureController : MonoBehaviour
{
    private float speed = 12.5f;
    private Rigidbody rigidBody;
    private GameObject player;
    private MainBoatController playerScript;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Boat");
        playerScript = player.GetComponent<MainBoatController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the treasure object towards the player when inhaling
        if (playerScript.inhalePhase && playerScript.inhaleIsOn && playerScript.inhaleDuration > 0.4f)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            transform.Translate(-lookDirection * speed * Time.deltaTime);
            if (transform.position.y < -5)
            {
                Destroy(gameObject);
            }
        }
    }
}
