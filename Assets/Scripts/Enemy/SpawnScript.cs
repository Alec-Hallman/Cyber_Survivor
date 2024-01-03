using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemyObjects;
    private float time;
    public float spawnRate;
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        GetCurrentTime();
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.realtimeSinceStartup - time) > spawnRate ){
            Vector2 playerPositio = player.transform.position;
            float angle = Random.Range(0,2 * Mathf.PI);
             Vector2 randPosition = playerPositio + (new Vector2( Mathf.Cos(angle), Mathf.Sin(angle)) * 12);
             GameObject enemyObject = enemyObjects[Random.Range(0,enemyObjects.Length)];
            Instantiate(enemyObject, randPosition, transform.rotation);
            GetCurrentTime();
            //Debug.Log("Spawned");
        }
    }
    void GetCurrentTime(){
        time = Time.realtimeSinceStartup;
    }
}
