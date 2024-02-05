using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Queue<Transform> enemies;
    void Start()
    {
        enemies = new Queue<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("enemy queue count: " + enemies.Count);
    }
    public Transform GetEnemy(){
        Transform tempTransform = enemies.Peek();
        if(tempTransform == null){
            tempTransform = CleanQueue();
        }
        return tempTransform;
    }
    void OnTriggerStay2D(Collider2D hit){
        if(!enemies.Contains(hit.gameObject.transform)){
            enemies.Enqueue(hit.gameObject.transform);
        }
    }
    private Transform CleanQueue(){
        Transform tempTransform = enemies.Dequeue();
        while(tempTransform != null && enemies.Count > 0){
            tempTransform = enemies.Peek();
            if(tempTransform == null){
                tempTransform = enemies.Dequeue();
            }
        }
        return tempTransform;
    }
}
