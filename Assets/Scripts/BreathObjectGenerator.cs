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
    private bool firstCoinSpawn = false;
    private bool inhaleSpawned = false;
    private bool exhaleSpawned = false;
    private float initialCoinDistance = 20f;
    private float initialTreasureDistance;
    private float remainingCoinDistance = 0f;

    private bool isCoroutineExecutingTreasure = false;
    private bool isCoroutineExecutingCoin = false;
    private bool isCoroutineExecutingCoinDestroy = false;
    private bool isCoroutineExecutingTreasureDestroy = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Boat");
        playerScript = player.GetComponent<MainBoatController>();
        initialTreasureDistance = (51 / 3) * playerScript.inhaleTargetTime;
    }


    // Update is called once per frame
    void Update()
    {
        if (!playerScript.gameOver)
        {
            if (playerScript.inhalePhase)
            {
                //Destroy(GameObject.FindGameObjectWithTag("Coin"));
                //Destroy(GameObject.FindGameObjectWithTag("Coin Two"));
                StartCoroutine(DestroyCoins());
                if (!inhaleSpawned)
                {
                    StartCoroutine(SpawnTreasureItems());
                }
            }
            if (playerScript.exhalePhase)
            {
                //Destroy(GameObject.FindGameObjectWithTag("Treasure"));
                StartCoroutine(DestroyTreasure());
                if (!exhaleSpawned)
                {
                    if (!firstCoinSpawn)
                    {
                        StartCoroutine(SpawnCoinItems());
                        //SpawnFirstCoin();
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
                            remainingCoinDistance = 0f;
                            exhaleSpawned = true;
                        }
                    }
                }
            }
        }
    }

    // Spawn the first coin in front of the boat.
    private void SpawnFirstCoin()
    {
        Vector3 playerPosition = transform.position;
        // Need cross product to produce coins in front of boat.
        Vector3 playerForward = Vector3.Cross(transform.forward, new Vector3(0, 1, 0));
        Quaternion playerRotation = Quaternion.Euler(90, 180, 0);
        Vector3 spawnPosition = new Vector3(RandomXPosition(), playerPosition.y, playerPosition.z) + new Vector3(0,0,1) * initialCoinDistance;
        Instantiate(coinOne, spawnPosition, playerRotation);
        firstCoinSpawn = true;
        inhaleSpawned = false;
    }

    // Spawn exhaleDuration number of coins one after another
    private void SpawnRemainingCoins()
    {
        // Need cross product to produce coins in front of boat.
        Vector3 playerForward = Vector3.Cross(transform.forward, new Vector3(0, 1, 0));
        Quaternion playerRotation = Quaternion.Euler(90, 180, 0);
        if (coinCount < playerScript.exhaleTargetTime)
        {
            remainingCoinDistance += 15;
            // Spawn the other coins right behind the first coin spawned.
            Vector3 spawnPosition = GameObject.FindGameObjectWithTag("Coin").transform.position + new Vector3(0, 0, 1) * remainingCoinDistance;
            Instantiate(remainingCoins, spawnPosition, playerRotation);
            coinCount++;
        }
    }

    private void SpawnTreasure()
    {
        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        // Need cross product to produce coins in front of boat.
        Vector3 playerForward = Vector3.Cross(transform.forward, new Vector3(0, 1, 0));
        Quaternion playerRotation = Quaternion.Euler(0, 180, 0);
        Vector3 spawnPosition = playerPosition + new Vector3(0, 0, 1) * initialTreasureDistance;
        Instantiate(treasure, spawnPosition, playerRotation);
        inhaleSpawned = true;
        exhaleSpawned = false;
        playerScript.cycleCounter += 1;
    }

    private float RandomXPosition()
    {
        return Random.Range(-25, 25);
    }

    private IEnumerator SpawnCoinItems()
    {

        if (isCoroutineExecutingCoin)
        {
            yield break;
        }
        isCoroutineExecutingCoin = true;
        yield return new WaitForSeconds(2.5f);
        SpawnFirstCoin();
        isCoroutineExecutingCoin = false;
    }


    private IEnumerator DestroyCoins()
    {
        if(isCoroutineExecutingCoinDestroy)
        {
            yield break;
        }
        isCoroutineExecutingCoinDestroy = true;
        yield return new WaitForSeconds(0.8f);
        Destroy(GameObject.FindGameObjectWithTag("Coin"));
        Destroy(GameObject.FindGameObjectWithTag("Coin Two"));
        isCoroutineExecutingCoinDestroy = false;
    }

    private IEnumerator SpawnTreasureItems()
    {
        if (isCoroutineExecutingTreasure)
        {
            yield break;
        }
        isCoroutineExecutingTreasure = true;
        yield return new WaitForSeconds(3.5f);
        SpawnTreasure();
        isCoroutineExecutingTreasure = false;
    }

    private IEnumerator DestroyTreasure()
    {
        if (isCoroutineExecutingTreasureDestroy)
        {
            yield break;
        }
        isCoroutineExecutingTreasureDestroy = true;
        yield return new WaitForSeconds(0.8f);
        Destroy(GameObject.FindGameObjectWithTag("Treasure"));
        isCoroutineExecutingTreasureDestroy = false;
    }
}
