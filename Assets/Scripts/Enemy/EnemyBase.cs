using System.Collections;
using System.Collections.Generic;
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
    public bool walking;
    public GameObject[] xpObjects;
    private Vector2 ZERO = new Vector2(0,0);
    public bool tracking;

    // Start is called before the first frame update
    public void EnemyStart()
    {
        player = GameObject.Find("Player");
        UI = GameObject.Find("Canvas");
        if(!tracking){
            direction = (player.transform.position - transform.position).normalized;
        }

        
    }

    // Update is called once per frame
    public void EnemyMoveUpdate()
    {
        transform.right = player.transform.position - transform.position;
        if(walking){
            if(tracking){
                direction = (player.transform.position - transform.position).normalized;
            }
            GetComponent<Rigidbody2D>().velocity = direction * walkSpeed;
        } else{
            GetComponent<Rigidbody2D>().velocity = ZERO;
        }
       


    }
    void OnTriggerEnter2D(Collider2D hit){
        if(hit.gameObject.tag == "Player"){
            //Debug.Log("Hitting player");
            hittingPlayer = true;
            GetCurrentTime();

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
}
