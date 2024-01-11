using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float health;
    public float attackSpeed;
    public float walkSpeed;
    public float damage;
    public bool hittingPlayer;
    public GameObject player = null;
    private Vector2 direction;
    private GameObject UI;
    public float time;
    private float hackTime;
    public bool walking;
    public GameObject[] xpObjects;
    private Vector2 ZERO = new Vector2(0,0);
    public bool tracking;
    protected bool hacked;

    // Start is called before the first frame update
    public void EnemyStart()
    {
        hacked = false;
        player = GameObject.Find("Player");
        UI = GameObject.Find("Canvas");
        if(!tracking){
            direction = (player.transform.position - transform.position).normalized;
        }

        
    }

    // Update is called once per frame
    public void EnemyMoveUpdate()
    {
        if(walking && !hacked){
            transform.right = player.transform.position - transform.position;
            if(tracking){
                direction = (player.transform.position - transform.position).normalized;
            }
            GetComponent<Rigidbody2D>().velocity = direction * walkSpeed;
        } else if (!hacked){
            GetComponent<Rigidbody2D>().velocity = ZERO;
        } else{
            Debug.Log("Im hacked asf");
        }
       


    }
    void OnTriggerEnter2D(Collider2D hit){
        if(hit.gameObject.tag == "Player"){
            //Debug.Log("Hitting player");
            hittingPlayer = true;
            GetCurrentTime();

        }
        //Debug.Log("Hit: " + hit.gameObject.name);
        if(hit.gameObject.name.Contains("Hack")){
            Debug.Log("hacked");
            hacked = true;
            GetComponent<Rigidbody2D>().velocity = ZERO;
            Invoke("StopHack", 2f);
        }
    }
    void OnTriggerExit2D(Collider2D hit){
        if(hit.gameObject.tag == "Player"){
            hittingPlayer = false;
        }
    }
    public void GetCurrentTime(){
        time = Time.realtimeSinceStartup;
    }
    public void takeDamage(float damage){
        health -= damage;
        UI.GetComponent<UIManager>().DisplayHit(damage,this.gameObject);
        if(health <= 0){
            Died();
        }
    } 
    void Died(){
        GameObject xp = Instantiate(xpObjects[Random.Range(0,xpObjects.Length)]);
        xp.transform.position = transform.position;
        Destroy(gameObject);
    }
    void StopHack(){
        Debug.Log("Stopping Hack");
        hacked = false;
    }
}
