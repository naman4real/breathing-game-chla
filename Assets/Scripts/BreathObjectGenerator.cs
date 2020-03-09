using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathObjectGenerator : MonoBehaviour
{
    public GameObject coinOne;
    public GameObject remainingCoins;
    public GameObject treasure;

    private GameObject player;
    private MainBoatController playerScript;

    private int coinCount = 1;
    private float startDelay = 20f;
    private bool firstCoinSpawn = false;
    private bool inhaleSpawned = false;
    private bool exhaleSpawned = false;
    private float initialCoinDistance = 75f;
    private float initialTreasureDistance = 75f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Boat");
        playerScript = player.GetComponent<MainBoatController>();
    }


    // Update is called once per frame
    void Update()
    {
        //SpawnItems();
        //Debug.Log(firstCoinSpawn);
        if (playerScript.inhalePhase == true)
        {
            if (!inhaleSpawned)
            {
                SpawnTreasure();
            }
        }
        if (playerScript.exhalePhase)
        {
            if (!exhaleSpawned)
            {
                if (!firstCoinSpawn)
                {
                    SpawnFirstCoin();
                }
                // Spawn the remaining coins based on the exhale duration.
                else
                {
                    SpawnRemainingCoins();
                    // Reset the coin flags.
                    if (coinCount == playerScript.exhaleTargetTime)
                    {
                        coinCount = 1;
                        firstCoinSpawn = false;
                        initialCoinDistance = 75f; 
                        exhaleSpawned = true;
                    }
                }
            }
        }
    }

    private void SpawnItems()
    {
       
    }

    // Spawn the first coin in front of the boat.
    private void SpawnFirstCoin()
    {
        Vector3 playerPosition = transform.position;
        // Need cross product to produce coins in front of boat.
        Vector3 playerForward = Vector3.Cross(transform.forward, new Vector3(0, 1, 0));
        Quaternion playerRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        Vector3 spawnPosition = playerPosition + playerForward * initialCoinDistance;
        Instantiate(coinOne, spawnPosition, playerRotation);
        firstCoinSpawn = true;
        inhaleSpawned = false;
    }

    // Spawn exhaleDuration number of coins one after another
    private void SpawnRemainingCoins()
    {
        Vector3 playerPosition = transform.position;
        // Need cross product to produce coins in front of boat.
        Vector3 playerForward = Vector3.Cross(transform.forward, new Vector3(0, 1, 0));
        Quaternion playerRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        if (coinCount < playerScript.exhaleTargetTime)
        {
            initialCoinDistance += 50;
            Vector3 spawnPosition = playerPosition + playerForward * initialCoinDistance;
            Instantiate(remainingCoins, spawnPosition, playerRotation);
            coinCount++;
        }
    }

    private void SpawnTreasure()
    {
        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        // Need cross product to produce coins in front of boat.
        Vector3 playerForward = Vector3.Cross(transform.forward, new Vector3(0, 1, 0));
        Quaternion playerRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        Vector3 spawnPosition = playerPosition + playerForward * initialTreasureDistance;
        Instantiate(treasure, spawnPosition, playerRotation);
        inhaleSpawned = true;
        exhaleSpawned = false;
    }
}
