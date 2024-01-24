using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemyObjects;
    public GameObject swarmEnemy;
    private float time;
     private float time2;
    private float swarmTimer;
    public float spawnRate;
    private GameObject player;
    public bool swarm;
    public float swarmDurration;
    private int difficulty;
    private bool positionSet;
    private float swarmSpawnTimer;
    private float healthMultiplier = 1;
    private Vector2 randCircularSpawn;
    public float waveDurration;
    private bool wave;
    private GameObject waveObject;
    void Start()
    {
        waveObject = enemyObjects[0];
        wave = false;
        swarmTimer = Random.Range(10, 30);
        swarmSpawnTimer = Time.realtimeSinceStartup;
        positionSet = false;
        player = GameObject.Find("Player");
        GetCurrentTime();
        swarm = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!(Time.timeScale == 0)){
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if(((Time.realtimeSinceStartup - time) > spawnRate ) && enemies.Length < 100){
                Vector2 playerPositio = player.transform.position;
                float angle = Random.Range(0,2 * Mathf.PI);
                Vector2 randPosition = playerPositio + (new Vector2( Mathf.Cos(angle), Mathf.Sin(angle)) * 12);
                GameObject enemyObject = enemyObjects[Random.Range(0,enemyObjects.Length)];
                GameObject tempEnemy;
                if(!wave){
                    tempEnemy = Instantiate(enemyObject, randPosition, transform.rotation);

                } else{
                    tempEnemy = Instantiate(waveObject,randPosition,transform.rotation);
                }
                tempEnemy.GetComponent<EnemyBase>().Balancing(healthMultiplier);
                GetCurrentTime();
                //Debug.Log("Spawned");
            }
            if((Time.realtimeSinceStartup - swarmSpawnTimer) > swarmTimer){
                //Debug.Log("Starting Swarm");
                swarm = true;
                swarmTimer = Random.Range(10, 30);
                swarmSpawnTimer = Time.realtimeSinceStartup;
            }
        }
    }
    void FixedUpdate(){
        if(swarm){
            StartSwarm();
        }
    }
    void GetCurrentTime(){
        time = Time.realtimeSinceStartup;
    }
    void GetSwarmTime(){
        time2 = Time.realtimeSinceStartup;
    }
    void StartSwarm(){
        if(Time.realtimeSinceStartup - time2 < swarmDurration && positionSet){
            Instantiate(swarmEnemy, randCircularSpawn, transform.rotation);
        }
        else if(!positionSet){
            SwarmPosition();
        }
        else{
            swarm = false;
            positionSet = false;
        }

    }
    void SwarmPosition(){
            GetSwarmTime();
            Vector2 playerPositio = player.transform.position;
            float angle = Random.Range(0,2 * Mathf.PI);
            Vector2 randPosition = playerPositio + (new Vector2( Mathf.Cos(angle), Mathf.Sin(angle)) * 12);
           randCircularSpawn = randPosition + Random.insideUnitCircle * 1;
           positionSet = true;
            
    }
    public void IncreaseRate(){
        spawnRate *= 0.9f;
    }
    public void IncreaseDifficulty(int ammount){
        difficulty += ammount;
//        Debug.Log("Difficulty: " + difficulty);
        if(difficulty % 10 == 0){
          //  Debug.Log("Increasing health");
            healthMultiplier += 0.1f;
        }
    }
    public void StartWave(string enemyName, float durration){
        waveDurration = durration;
        Debug.Log("Starting a wave of: " + enemyName);
        int counter = 0;
        while( waveObject != null && waveObject.name != enemyName){
            if(enemyObjects[counter].name == enemyName){
                waveObject = enemyObjects[counter];
            }
            counter ++;
        }
        wave = true;
        Invoke("EndWave", 2f);
    }
    void EndWave(){
        wave = false;
    }
}
