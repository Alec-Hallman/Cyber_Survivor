using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnScript : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> enemyObjects;
    public Queue test;
    public GameObject swarmEnemy;
    private float time;
     private float time2;
    private float swarmTimer;
    [SerializeField] private GameObject weakEnemy;
    public float spawnRate;
    private GameObject player;
    public bool swarm;
    public float swarmDurration;
    private float waveTimer;
    private int difficulty;
    private bool positionSet;
    private float swarmSpawnTimer;
    private float healthMultiplier = 1f;
    private Vector2 randCircularSpawn;
    public float waveDurration;
    private float waveTime;
    private bool wave;
    private GameObject waveObject;
    private Vector2 oldPosition;
    [SerializeField]private List<GameObject> easyEnemies;
    [SerializeField]private List<GameObject> mediumEnemies;
    [SerializeField]private List<GameObject> hardEnemies;
    [SerializeField]private List<GameObject> currentSelectionEnemies;
    public int enemyCount;
    private bool med;
    private bool hard;
    private bool newPosition;
    private float maxSwarmTime;
    void Start()
    {
        newPosition = true;
        NewWaveTimer();
        waveTime = Time.realtimeSinceStartup;
        med = false;
        hard = false;
        maxSwarmTime = 120;
        easyEnemies = new List<GameObject>();
        mediumEnemies = new List<GameObject>();
        hardEnemies = new List<GameObject>();
        currentSelectionEnemies = new List<GameObject>();

        waveObject = enemyObjects[0];
        wave = false;
        swarmTimer = Random.Range(10, maxSwarmTime);
        swarmSpawnTimer = Time.realtimeSinceStartup;
        positionSet = false;
        player = GameObject.Find("Player");
        GetCurrentTime();
        swarm = false;
        foreach(GameObject enemy in enemyObjects){
            if(enemy.tag.Contains("Easy")){
                easyEnemies.Add(enemy);
            } else if(enemy.tag.Contains("Medium")){
                mediumEnemies.Add(enemy);
            } else{
                hardEnemies.Add(enemy);
            }
        }
        currentSelectionEnemies = easyEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0){
            if(((Time.realtimeSinceStartup - time) > spawnRate ) && enemyCount < 100){
                Vector2 playerPositio = player.transform.position;
                float angle = Random.Range(0,2 * Mathf.PI);
                Vector2 randPosition;
                if(newPosition){
                    randPosition = playerPositio + (new Vector2( Mathf.Cos(angle), Mathf.Sin(angle)) * 12);
                    oldPosition = randPosition;
                } else{
                    randPosition = oldPosition;
                }
                GameObject enemyObject = currentSelectionEnemies[Random.Range(0,currentSelectionEnemies.Count)];
                GameObject tempEnemy;
                if(!wave){
                    tempEnemy = Instantiate(enemyObject, randPosition, transform.rotation);
                    enemyCount++;

                } else{
                    tempEnemy = Instantiate(waveObject,randPosition + Random.insideUnitCircle * 2,transform.rotation);
                    newPosition = false;
                    enemyCount++;
                }
                EnemyBase tempScript = tempEnemy.GetComponent<EnemyBase>();
                tempScript.Balancing(healthMultiplier);
                tempScript.spawnScript = this;
                GetCurrentTime();
                //Debug.Log("Spawned");
            }
            if((Time.realtimeSinceStartup - swarmSpawnTimer) > swarmTimer){
                //Debug.Log("Starting Swarm");
                swarm = true;
                swarmTimer = Random.Range(10, 30);
                swarmSpawnTimer = Time.realtimeSinceStartup;
            }
            if((Time.realtimeSinceStartup - waveTime) > waveTimer){
                StartWave("",2);  //If its been long enough start a wave of a random enemy          
            }
        }
    }
    void FixedUpdate(){
        if(swarm && Time.timeScale != 0){
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
        if(Time.realtimeSinceStartup - time2 < swarmDurration && positionSet && Time.timeScale != 0){
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
            maxSwarmTime *= 0.99f;
            healthMultiplier += 0.1f;
            if(enemyObjects.Contains(weakEnemy)){
                enemyObjects.Remove(weakEnemy);
            }
            
        }
    }
    public void StartWave(string enemyName, float durration){
        if(!wave){
            spawnRate *= 0.6f;
            waveDurration = durration;
            //Debug.Log("Starting a wave of: " + enemyName);
            if(enemyName != ""){ //if given an empty string start a random wave of enemies
                int counter = 0;
                while( waveObject != null && waveObject.name != enemyName){
                    if(enemyObjects[counter].name == enemyName){
                        waveObject = enemyObjects[counter];
                    }
                    counter ++;
                }
            } else{
                while(waveObject != null && waveObject.tag.Contains("Hard")){
                    waveObject = enemyObjects[Random.Range(0,currentSelectionEnemies.Count)];

                }
            }
            wave = true;
            Debug.Log("Starting a wave");
            Invoke("EndWave", waveDurration);
        }
    }
    void EndWave(){
        spawnRate *= 1.4f;
        wave = false;
        newPosition = true;
        waveTime = Time.realtimeSinceStartup;
        NewWaveTimer();
    }
    private void CombineLists(List<GameObject> list){
        //List 1 = list to combine into enemy current list
        currentSelectionEnemies.AddRange(list);
    }
    private void RemoveList(List<GameObject> list){
        //list 1 = list to remove from enemy current list
        List<GameObject> tempList = currentSelectionEnemies.Except(list).ToList();
        currentSelectionEnemies = tempList;
    }
    void NewWaveTimer(){
        waveTimer = Random.Range(60, 120);
    }
    public void EnemyShift(){
        if(!med){
            med = true;
            CombineLists(mediumEnemies);
        }else if(!hard){
            hard = true;
            CombineLists(hardEnemies);
            
        }
    }
}
