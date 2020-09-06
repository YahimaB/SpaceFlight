using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour
{
    //Spawn properties
    [Header("Spawn hazards properties")]
    public GameObject blackhole;
    public GameObject hazard;
    public Vector3 hazardSpawnValues;
    public float spawnWait;
    public float startWait;
    //Spawn pickups properties
    [Header("Spawn pickups properties")]
    public GameObject pickups;
    public float minTimeBeforeSpawn;
    public float maxTimeBeforeSpawn;
    public Vector3 pickUpSpawnValues;
    //Buffs
    [Header ("Buffs")]
    public GameObject shield;
    //GUI
    [Header("GUI")]
    public GameObject inGamePanel;
    public Text scoreText;
    public Text gemCounter;
    public GameObject losePanel;
    public Text finalScoreText;
    public GameObject newRecordMessage;
    public GameObject menuPanel;
    public GameObject shopPanel;
    public static bool scoreIsMore =false;
    //Planet respawn
    [Header("Planet")]
    public GameObject planet;
    [Header("Link Manager")]
    public LinkManager linkManager;

    private bool gameOver = false;
    private int score = 0;
    private int gems = 0;
    private int gamesPlayed = 0;

    private SaveLoadData saveLoadData = new SaveLoadData();
    private PlayerController player = default;

    private Coroutine spawnWaves;
    private Coroutine spawnPickUps;

	void Start()
    {
        scoreIsMore = false;
        planet.SetActive(true);
        inGamePanel.SetActive(true);
        gameOver = false;
        score = 0;
        gems = 0;
        losePanel.SetActive(false);
        UpdateScore();
        spawnWaves = StartCoroutine(SpawnWaves());
        spawnPickUps = StartCoroutine(SpawnPickUps());
    }

	private void Update()
	{
        if(gameOver)
        ClearJunk();
	}

	public void StartGame()
    {
        if (gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(true);
            return;
        }
        this.Start();
    }

    public void ScoreMore(){
        scoreIsMore = true;
    }

    public void AddScore(bool isGem, int newScoreValue)
    {
        if (!isGem)
        {
            score += newScoreValue;
        }
        else
        {
            gems += newScoreValue;
        }
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score:\n" + score;
        gemCounter.text = "Gems:\n" + gems;
    }

    public void CreateShield(){
        Instantiate(shield, new Vector3(0, 0, -7), Quaternion.identity);
    }

    public void GameOver()
    {
        StopCoroutine(spawnWaves);
        StopCoroutine(spawnPickUps);
        gamesPlayed++;
        if (gamesPlayed % 3 == 0)
        {
            linkManager.ShowUsualAd();
        }
        inGamePanel.SetActive(false);
        losePanel.SetActive(true);
        finalScoreText.text = score.ToString();
        //print("Game Over");
        gameOver = true;

        //print(saveLoadData.LoadCount(true));
        //print(gems);
        saveLoadData.SaveScore(score, gems);
        if (scoreIsMore) newRecordMessage.SetActive(true);
        print(saveLoadData.LoadCount(false));
    }

    public void RestartGame(){
        print("Restart button works");
        newRecordMessage.SetActive(false);
        StartGame();
    }

    public void ClearJunk(){
        //Убрать мусор (астероиды, пикапы) с карты
        string[] junkTags = { "Asteroid", "Gem", "Shield" };
        foreach (string junkTag in junkTags)
        {
            GameObject[] junk = GameObject.FindGameObjectsWithTag(junkTag);
            foreach (GameObject thing in junk)
            {
                Destroy(thing);
            }
        }
    }

    //IEnumerator SpawnWaves()
    //{
    //    yield return new WaitForSeconds(startWait);
    //    while (true) 
    //    {
    //        bool ender = false;
    //        for (int i = 0; i < hazardCount; i++)
    //        {
    //            Vector3 spawnPosition = new Vector3(Random.Range(-hazardSpawnValues.x, hazardSpawnValues.x), hazardSpawnValues.y, hazardSpawnValues.z);
    //            Quaternion spawnRotation = Quaternion.identity;
    //            Instantiate(hazard, spawnPosition, spawnRotation);
    //            yield return new WaitForSeconds(spawnWait);

    //            if (gameOver)
    //            {
    //                ender = true;
    //                break;
    //            }
    //        }
    //        if(ender){
    //            break;
    //        }
    //    }
    //}

    IEnumerator SpawnWaves()
    {
        print("SpawnWaaves started");
        yield return new WaitForSeconds(startWait);
        int hazzardNumber = 0;
        float extraRate = 0.0f;
        while (!gameOver)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-hazardSpawnValues.x, hazardSpawnValues.x),
                                                hazardSpawnValues.y-2,
                                                Random.Range(hazardSpawnValues.z, 13f));
            Quaternion spawnRotation = Quaternion.identity;

            Instantiate (blackhole, spawnPosition, Quaternion.identity);

            spawnPosition = new Vector3(spawnPosition.x,
                                                hazardSpawnValues.y,
                                                spawnPosition.z);
            yield return new WaitForSeconds(0.4f);
            Instantiate(hazard, spawnPosition, spawnRotation);
            hazzardNumber++;
            if(hazzardNumber>0 && hazzardNumber%10 ==0){
                extraRate -= 0.05f;
                print("spawn rate incresed to " + spawnWait+extraRate);
            }
            yield return new WaitForSeconds(spawnWait+extraRate);
        }
        print("SpawnWaaves stoped");

    }

    //IEnumerator SpawnPickUps()
    //{
    //    yield return new WaitForSeconds(startWait);
    //    while (true){
    //        float waitBeforeNext = Random.value*maxTimeBeforeSpawn;
    //        if(waitBeforeNext>minTimeBeforeSpawn){
    //            yield return new WaitForSeconds(waitBeforeNext);
    //            //random a value for different pickups
    //            float randomed = Random.Range(0, pickups.transform.childCount);
    //            int index = (int)Mathf.Round(randomed);
    //            //print("index = " + index + " count = " + randomed);
    //                GameObject nextPickUp = pickups.transform.GetChild(index).gameObject;
    //            float spawnPosX = Random.Range(pickUpSpawnValues.x, 5.0f);
    //            if (Random.value > 0.5f) spawnPosX *= -1;
    //            Vector3 spawnPosition = new Vector3(spawnPosX, pickUpSpawnValues.y, pickUpSpawnValues.z);
    //            Quaternion spawnRotation = nextPickUp.transform.rotation;
    //            Instantiate(nextPickUp, spawnPosition, spawnRotation);
    //        }
    //        if(gameOver){
    //            break;
    //        }
    //    }
    //}
    IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(startWait);
        while (!gameOver)
        {
            float waitBeforeNext = Random.value * maxTimeBeforeSpawn;
            if (waitBeforeNext > minTimeBeforeSpawn)
            {
                yield return new WaitForSeconds(waitBeforeNext);
                //random a value for different pickups
                float randomed = Random.Range(0, pickups.transform.childCount+1);
                int index = (int)Mathf.Round(randomed);
                //print("index = " + index + " count = " + randomed);
                if(index>0){
                    index = 1;
                }
                GameObject nextPickUp = pickups.transform.GetChild(index).gameObject;
                //float spawnPosX = Random.Range(pickUpSpawnValues.x, 5.0f);
                //if (Random.value > 0.5f) spawnPosX *= -1;
                Vector3 spawnPosition = new Vector3(pickUpSpawnValues.x, pickUpSpawnValues.y, pickUpSpawnValues.z);
                Quaternion spawnRotation = nextPickUp.transform.rotation;
                Instantiate(nextPickUp, spawnPosition, spawnRotation);
            }
        }
    }
}

