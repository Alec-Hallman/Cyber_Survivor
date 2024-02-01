using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;
    public float maxSpeed;
    public float searchTimer;
    public float orbitRadius;
    private float angle = 0f;
    private Vector2 previousPosition;
    public Queue<GameObject> enemyQueue;
    public float missleNumber;

    public GameObject missle;
    public float spawnTimer;
    private GameObject missleObject;
    private Queue<MissleObject> missleScript;
    private GameObject enemy;
    private GameObject enemy2;
    private float lastSpawn;
    public float range;
    bool spawn;
    private float spawnTime;
    void Start()
    {
        spawn = true;
        //Initialize queues
        missleScript = new Queue<MissleObject>();
        enemyQueue = new Queue<GameObject>();
        // Find player
        player = GameObject.Find("Player").GetComponent<Transform>(); 
        //Set previous Position (important for cirular movement)
        previousPosition = transform.position;
        //Set this object position = to the player's position
        this.transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement **
        Vector2 direction = (Vector2)transform.position - previousPosition;
        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        previousPosition = transform.position;
        Vector2 newPos = (Vector2) player.position + (new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * orbitRadius);
        transform.position = newPos;
        angle += Time.deltaTime * maxSpeed;
        //End of Movement **
        //if there isn't a current missleObject, it's been long enough, there are enemies in the queue, and there aren't missle compoenents in the script queue.
        if( spawn && (Time.realtimeSinceStartup - lastSpawn > spawnTimer) && missleScript.Count == 0){
                spawn = false;
                float tempTimer = 0.5f;
                Invoke("ClearEnemies",2.5f);
                for(int i = 0; i < missleNumber; i++){
                    Invoke("SpawnMissle", tempTimer);
                    tempTimer += 0.2f;
                }
                //Invoke("ShootMissle", 1f);
        }
    }
    private void MissleClean(){
        while( missleScript.Count != 0 && missleScript != null && missleScript.Peek() == null){
            missleScript.Dequeue();
        }
    }
    private void EnemyClean(){
        while(enemyQueue.Count != 0 && enemyQueue.Peek() == null){
            enemyQueue.Dequeue();
        }
    }
    void SpawnMissle(){
    //Reset the spawn timer
    lastSpawn = Time.realtimeSinceStartup;
    //Create a missle
    GameObject missleOb = Instantiate(missle);
    //temp script saver
    MissleObject missleObjectComponent = missleOb.GetComponent<MissleObject>();
    //if there are enemies in the queue (in range)
    EnemyClean();
    if (enemyQueue.Count > 0) {
        //temp remember the target
        GameObject enemyTarget = enemyQueue.Dequeue();
        //Set the target to the enemy
        if(missleObjectComponent != null){
            //Debug.Log("Setting to enemy");
            missleObjectComponent.enemyObject = enemyTarget;
            missleObjectComponent.targetName = enemyTarget.name;
            //missleObjectComponent.SetTarget(enemyTarget);
        }
    } else{
        Debug.Log("Setting Random");
        missleObjectComponent.Invoke("SetRandom",0.5f);
    }
    spawn = true;
    //Wait for 0.5 seconds before spawning the next missile
}
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag.Contains("Enemy")){
            if(!enemyQueue.Contains(col.gameObject)){
                enemyQueue.Enqueue(col.gameObject);
               // Debug.Log("Enqueue Length is now: " + enemyQueue.Count);

            }
        }
        if(col.gameObject.tag.Contains("Enemy") && enemy == null){
            //Debug.Log("Hit enemy");
            //Debug.Log("I hit an enemy and have invoked ShootMissle");
            EnemyClean();
            if(enemyQueue.Count != 0){
                enemy = enemyQueue.Dequeue();
            }

            // Debug.Log("invoking shoot");
            
        }
    }
    private void ClearEnemies(){
        enemyQueue.Clear();
    }
}
