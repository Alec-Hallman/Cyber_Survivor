using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    private bool fly;
    private Vector2 direction;
    private GameObject player;
    private Vector2 playerPosition;
    private float speed;
    // Start is called before the first frame update
    private float timer;
    void Start()
    {
        speed = 1f;
        fly = false;
        player = GameObject.Find("Player");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
     if(fly){
        if((Time.realtimeSinceStartup - timer) > 5f){
            transform.position = player.transform.position;
            Destroy(this.gameObject);
        }
        direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * (speed *= 1.05f);
        //transform.position = Vector2.Lerp(transform.position, playerPosition, (Time.deltaTime * 1.1f));
     }
     if(transform.position == player.transform.position){
        Destroy(this.gameObject);
     }
    }
    void OnTriggerEnter2D(Collider2D col){
        
        if(col.gameObject.name == "Player"){
            fly = true;
            GetComponent<BoxCollider2D>().isTrigger = false;
            timer = Time.realtimeSinceStartup;
        }
    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.name == "Player"){
            Destroy(gameObject);
        }
    }

}
