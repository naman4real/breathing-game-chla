using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathObjectGenerator : MonoBehaviour
{
    public GameObject coinOne;
    public GameObject remainingCoins;
    public GameObject treasure;

    public int cycles;
    public int exhaleDuration;
    public int inhaleDuration;
    public int cycleWaitTime;

    //private MainBoatController player;

    private int coinCount = 1;
    private float startDelay = 20f;
    private bool firstCoinSpawn = false;
    private bool inhaleState = true;
    private bool exhaleState = false;
    private float initialCoinDistance = 200f;
    private float initialTreasureDistance = 75f;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Boat").GetComponent<MainBoatController>();

        // Spawn a new first coin every waitTime seconds.
        //InvokeRepeating("spawnTreasure", waitTimeState(), cycleWaitTime);
        //InvokeRepeating("spawnFirstCoin", waitTimeState(), cycleWaitTime);
    }


    // Update is called once per frame
    void Update()
    {
        if (inhaleState)
        {
            InhaleCountDown();
        }
        if (exhaleState)
        {
            ExhaleCountDown();
        }

        // Spawn the remaining coins based on the exhale duration.
        if(firstCoinSpawn)
        {
            SpawnRemainingCoins();
            // Reset the coin flags.
            if(coinCount == exhaleDuration)
            {
                
                coinCount = 1;
                exhaleState = false;
                firstCoinSpawn = false;
                initialCoinDistance = 200f;
                inhaleState = true;
            }
        }
        
    }

    private IEnumerator InhaleCountDown()
    {
        Debug.Log(inhaleState);        
        yield return new WaitForSeconds(3);
        SpawnTreasure();
        inhaleState = false;
    }

    private IEnumerator ExhaleCountDown()
    {   
        yield return new WaitForSeconds(waitExhaleTimeState());
        SpawnFirstCoin();
        exhaleState = false;
    }

    private float waitExhaleTimeState()
    {
        //if (player.exhaleIsOn)
        {
            startDelay = 0f;
        }
        return startDelay;
    }

    // Spawn the first coin in front of the boat.
    private void SpawnFirstCoin()
    {
        Vector3 playerPosition = transform.position;
        // Need cross product to produce coins in front of boat.
        Vector3 playerForward = Vector3.Cross(transform.forward, new Vector3(0, 1, 0));
        Quaternion playerRotation = transform.rotation;
        Vector3 spawnPosition = playerPosition + playerForward * initialCoinDistance;
        Instantiate(coinOne, spawnPosition, playerRotation);
        firstCoinSpawn = true;
        
    }

    // Spawn exhaleDuration number of coins one after another
    private void SpawnRemainingCoins()
    {
        Vector3 playerPosition = transform.position;
        // Need cross product to produce coins in front of boat.
        Vector3 playerForward = Vector3.Cross(transform.forward, new Vector3(0, 1, 0));
        Quaternion playerRotation = transform.rotation;
        if (coinCount < exhaleDuration)
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
        Quaternion playerRotation = transform.rotation;
        Debug.Log(transform.rotation.x);
        Vector3 spawnPosition = playerPosition + playerForward * initialTreasureDistance;
        Instantiate(treasure, spawnPosition, playerRotation);
        Debug.Log("yes");
        inhaleState = false;
        exhaleState = true;
    }
}
