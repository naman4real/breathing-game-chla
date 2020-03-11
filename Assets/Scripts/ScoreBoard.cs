//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ScoreBoard : MonoBehaviour
//{
//    public float coinScore;
//    public float treasureScore;
//    public float totalCoins;
//    public float totalTreasure;

//    private MainBoatController player;

//    private Text inhaleScore;
//    private Text exhaleScore;

//    private ScoreBoard exhaleScoreCard;
//    private ScoreBoard inhaleScoreCard;
//    // Start is called before the first frame update
//    void Start()
//    {
//        inhaleScore = GameObject.FindGameObjectWithTag("Treasure Score").GetComponent<Text>();
//        exhaleScore = GameObject.FindGameObjectWithTag("Coin Score").GetComponent<Text>();
//        player = GameObject.FindGameObjectWithTag("Boat").GetComponent<MainBoatController>();

//        inhaleScoreCard = GameObject.FindGameObjectWithTag("Treasure Score").GetComponent<ScoreBoard>();
//        exhaleScoreCard = GameObject.FindGameObjectWithTag("Coin Score").GetComponent<ScoreBoard>();

//        totalCoins = player.exhaleTargetTime * player.cycles;
//        totalTreasure = player.cycles;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//        inhaleScore.text = "Treasure: " + treasureScore;
//        // Up[
//        exhaleScoreCard.treasureScore = treasureScore;
//        exhaleScore.text = "Coins: " + coinScore;
//        inhaleScoreCard.coinScore = coinScore;

//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public float coinScore;
    public float treasureScore;
    public float totalCoins;
    public float totalTreasure;

    private MainBoatController player;

    private Text inhaleScore;
    private Text exhaleScore;
    private Text finalScore;

    private ScoreBoard exhaleScoreCard;
    private ScoreBoard inhaleScoreCard;
    private ScoreBoard finalScoreCard;
    // Start is called before the first frame update
    void Start()
    {
        inhaleScore = GameObject.FindGameObjectWithTag("Treasure Score").GetComponent<Text>();
        exhaleScore = GameObject.FindGameObjectWithTag("Coin Score").GetComponent<Text>();
        finalScore = GameObject.FindGameObjectWithTag("Final Score").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Boat").GetComponent<MainBoatController>();

        inhaleScoreCard = GameObject.FindGameObjectWithTag("Treasure Score").GetComponent<ScoreBoard>();
        exhaleScoreCard = GameObject.FindGameObjectWithTag("Coin Score").GetComponent<ScoreBoard>();
        finalScoreCard = GameObject.FindGameObjectWithTag("Final Score").GetComponent<ScoreBoard>();

        totalCoins = player.exhaleTargetTime * player.cycles;
        totalTreasure = player.cycles;

        //finalScore.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.gameOver)
        {
            //finalScore.gameObject.SetActive(true);
            finalScore.text = "Final Score: " + (treasureScore + coinScore) + "/" + (totalCoins + totalTreasure);
        }
        else
        {
            finalScore.text = "";
        }
        inhaleScore.text = "Treasure: " + treasureScore;
        exhaleScore.text = "Coins: " + coinScore;
    }
}
