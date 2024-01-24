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
    private float distance;
    private bool poisoned = false;
    private float pDamage;
    public bool radioactiveBool;
    private float pTimer;
    private float pDurration;
    private Color startColor;
    private bool dead;
    protected bool playerInRange;
    private bool knockBack;

    // Start is called before the first frame update
    public void EnemyStart()
    {
        playerInRange = false;
        knockBack = false;
        dead = false;
        startColor = this.GetComponent<SpriteRenderer>().color;
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
        distance = Vector2.Distance(player.transform.position, this.transform.position);
        if(distance > 20f){
            Destroy(this.gameObject);
        }
        //Debug.Log(distance);
        if(walking && !hacked){
            transform.right = player.transform.position - transform.position;
            if(!knockBack){ //Tracking refers to enemies that will walk directly towards the player, the swarm is the only non tracking enemy atm.
                if(tracking){
                    direction = (player.transform.position - transform.position).normalized;
                }
                GetComponent<Rigidbody2D>().velocity = direction * walkSpeed;

            } else if(knockBack){
                direction = -1 * (player.transform.position - transform.position).normalized; //If the enemy is going to be knocked back make the direction towards the player negative so it moves away from the player.
                GetComponent<Rigidbody2D>().velocity = direction * 1f; //The 1f here is what controls how fast the enemy moves when it is knocked back.

            }
        } else if (!hacked){
            GetComponent<Rigidbody2D>().velocity = ZERO;
        }
        if(poisoned && (Time.realtimeSinceStartup - pTimer) > 1f && Time.timeScale != 0){
            takeDamage(pDamage, true);
            pDurration -= 1;
            GetPTime();
            if(pDurration <= 0){
                poisoned = false;
                this.GetComponent<SpriteRenderer>().color = startColor;
            }
        }
       


    }
    void OnTriggerEnter2D(Collider2D hit){
        if(hit.gameObject.tag == "Player"){
            //Debug.Log("Hitting player");
            hittingPlayer = true;
            playerInRange = true;
            //GetCurrentTime();

        }
        //Debug.Log("Hit: " + hit.gameObject.name);
        if(hit.gameObject.name.Contains("Hack")){
            //Debug.Log("hacked");
            hacked = true;
            GetComponent<Rigidbody2D>().velocity = ZERO;
            Invoke("StopHack", 0.5f);
        }
    }
    void OnCollisionEnter2D(Collision2D hit){
        if(hit.gameObject.tag == "Player"){
            //Debug.Log("Hitting player");
            hittingPlayer = true;
            //GetCurrentTime();

        }
        if(radioactiveBool){
            if(hit.gameObject.tag == "Enemy"){
                //Debug.Log("Poison Enemy");
                hit.gameObject.GetComponent<EnemyBase>().Poisoned(pDurration, pDamage, false);
            }
        }
        if(hit.gameObject.tag == "SendBack"){
            this.takeDamage(5,false);
            Destroy(hit.gameObject);

        }
    }
    void OnTriggerExit2D(Collider2D hit){
        if(hit.gameObject.tag == "Player"){
            hittingPlayer = false;
            playerInRange = false;
        }
    }
    public void GetCurrentTime(){
        time = Time.realtimeSinceStartup;
    }
    public void takeDamage(float damage, bool poison){
        
        if(poison){
            if((health - damage) <= 0){ //If the enemy is going to die from poison, don't, but still display the number.
                UI.GetComponent<UIManager>().DisplayHit(damage,this.gameObject, false, true);
            } else{
                health -= damage;
                UI.GetComponent<UIManager>().DisplayHit(damage,this.gameObject, false, true);
            }
            
        } else{
            if(!poisoned){
                gameObject.GetComponent<SpriteRenderer>().color = Color.white; //if not poisoned don't change the enemies colour.
            }
            knockBack = true;
            Invoke("StopKnockBack",0.1f); //0.1f controls how long the knockback
            health -= damage;
            UI.GetComponent<UIManager>().DisplayHit(damage,this.gameObject, false, false);
        }
        if(health <= 0 && !dead){
            dead = true;
            Died();
        }
    } 
    void Died(){
        GameObject xp = Instantiate(xpObjects[Random.Range(0,xpObjects.Length)]);
        xp.transform.position = transform.position;
        Destroy(gameObject);
    }
    void StopHack(){
       // Debug.Log("Stopping Hack");
        hacked = false;
    }
    public void Poisoned(float durration, float damage, bool radioactive){
        if(!poisoned){
            GetPTime();
            pDurration = durration;
            poisoned = true;   
            pDamage = damage;
            radioactiveBool = radioactive;
            this.GetComponent<SpriteRenderer>().color = Color.green;    
        }
    }
    void GetPTime(){
        pTimer = Time.realtimeSinceStartup;
    }
    public void Balancing(float multiplier){
        health *= multiplier;
        damage *= multiplier;
        //Debug.Log("multiplier: " +multiplier + "Damage Multiplier: " + multiplier/1.5f);
    }
    private void StopKnockBack(){
        if(!poisoned){
            this.GetComponent<SpriteRenderer>().color = startColor; //if the enemy is poisoned, aka green, then don't make them change colour.
        }
        knockBack = false;
    }

}


//poison max how it will work:
//When the enemy enters a trigger that has the tag enemy, if it's "radioactive" bool is true, then if the enemy is in that trigger for say, 5 seconds, it will make this enemy radioactive,
//radiactive won't be applied to enemies until an enemy is hit by a player who has poisoned maxed out.
//This way I can avoid triggering a bool or some kind of method on every single enemy once the player gains the maxed ability.