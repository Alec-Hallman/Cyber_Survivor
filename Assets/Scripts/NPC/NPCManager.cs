using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Queue<Transform> enemies;
    public float health;
    void Start()
    {
        enemies = new Queue<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
       //Debug.Log("enemy queue count: " + enemies.Count);
    }
    public Transform GetEnemy(){
        if(enemies.Count > 0){ //If the queue has something in it.
            Transform tempTransform = enemies.Peek(); // Giving a peek of the top transform in the queue, allowing multiple allies to target the same enemy
            if(tempTransform == null){ // if the given transform no longer exists
                tempTransform = CleanQueue(); //Clean the queue and get a new one
            }
            return tempTransform;// return the new transform
        } else{
            return null; // If the queue is empty, return null, the enemy will keep pinning this code until it finds an enemy. Which is bound to happen eventually.
        }
    }
    void OnTriggerStay2D(Collider2D hit){ //Every frame there's something in the queue.
        if(hit.gameObject.tag.Contains("Enemy") && !enemies.Contains(hit.gameObject.transform)){ // if the enemy doesnt contain the currently found item
            enemies.Enqueue(hit.gameObject.transform);
        }
    }
    private Transform CleanQueue(){
        Transform tempTransform = null;
        while(enemies.Count > 0 && tempTransform == null){
            tempTransform = enemies.Dequeue();
        }
        return tempTransform;
    }
}
