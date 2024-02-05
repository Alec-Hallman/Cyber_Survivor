using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public GameObject brainWashed;
    public bool hittingPuddle;
    public float health;
    public float attackSpeed;
    public float walkSpeed;
    public float damage;
    public bool hittingPlayer;
    public GameObject player = null;
    //private Transform playerPosition;
    private Vector2 direction;
    public float NPChealth;
    public SpawnScript spawnScript;
    private GameObject UI;
    public float time;
    private float hackTime;
    public bool walking;
    public bool tranced;
    public GameObject[] xpObjects;
    private Vector2 ZERO = new Vector2(0,0);
    public bool tracking;
    protected bool hacked;
    private float distance;
    public bool poisoned = false;
    public float npcChance;
    private float pDamage;
    public bool radioactiveBool;
    private float pTimer;
    private float pDurration;
    private Color startColor;
    private bool dead;
    protected bool playerInRange;
    private bool knockBack;
    public ChunkManager chunkScript;
    private SpriteRenderer objectRend;

    // Start is called before the first frame update
    public void EnemyStart()
    {
        //chunkScript = GameObject.Find("MapManager").GetComponent<ChunkManager>();
        //Debug.Log("chunk");
        //tranced = false;
        playerInRange = false;
        knockBack = false;
        dead = false;
        objectRend = this.GetComponent<SpriteRenderer>();
        startColor = objectRend.color;
        hacked = false;
        player = GameObject.Find("Player");
        UI = GameObject.Find("Canvas");
        hittingPuddle = false;
        if(!tracking){
            direction = (player.transform.position - transform.position).normalized;
        }

        
    }

    // Update is called once per frame
    public void EnemyMoveUpdate()
    {
        distance = Vector2.Distance(player.transform.position, this.transform.position);
        if(distance > 20f){
            Died();
        }
        //Debug.Log(distance);
        // if(walking && !hacked){
        //     Vector2 targetPosition = player.transform.position;
        //     this.transform.position = Vector2.Lerp(this.transform.position, targetPosition, walkSpeed);
        // }
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
                objectRend.color = startColor;
            }
        }
    }
    void FixedUpdate(){
        // if(Time.frameCount % 100 == 0){
        //     //Debug.Log("Chunk script name: " +chunkScript.name);
        //     if(chunkScript == null){
        //         Debug.Log("Chunk Script is null");
        //     }
        //     chunkScript.theMap.UpdateObjectChunk(gameObject);
        //     //chunk.Tostring();
        // }
    }
    void OnTriggerEnter2D(Collider2D hit){
        if(hit.gameObject.tag == "Player"){
            //Debug.Log("Hitting player");
            hittingPlayer = true;
            playerInRange = true;
            //GetCurrentTime();

        }
        //Debug.Log("Hit: " + hit.gameObject.name);
        // if(hit.gameObject.name.Contains("Hack")){
        //     Debug.Log("hacked");
        //     hacked = true;
        //     GetComponent<Rigidbody2D>().velocity = ZERO;
        //     Invoke("StopHack", 0.5f);
        // }
    }
    public void Hacked(){
        //Debug.Log("hacked");
        hacked = true;
        GetComponent<Rigidbody2D>().velocity = ZERO;
        objectRend.color = Color.yellow;
        Invoke("StopHack", 0.5f);
    }
    void StopHack(){
       // Debug.Log("Stopping Hack");
        hacked = false;
        objectRend.color = startColor;
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
        // if(hit.gameObject.tag == "SendBack"){
        //     ProjectileScript tempScript = hit.gameObject.GetComponent<ProjectileScript>();
        //     float damage = tempScript.damage;
        //     if(tempScript.poison){
        //         Poisoned(tempScript.pDurration, tempScript.pDamage, tempScript.radioactive);
        //     }
        //     this.takeDamage(damage,false);
        //     Destroy(hit.gameObject);
        // }
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
                objectRend.color = Color.white; //if not poisoned don't change the enemies colour.
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
        if(!this.gameObject.name.Contains("Swarm")){
            if(spawnScript == null){
                spawnScript = GameObject.Find("EnemyManager").GetComponent<SpawnScript>();
            }
            spawnScript.enemyCount = spawnScript.enemyCount - 1;
        }
        GameObject xp = Instantiate(xpObjects[Random.Range(0,xpObjects.Length)]);
        xp.transform.position = transform.position;
        Destroy(gameObject);
        if(tranced){
            Debug.Log("Spawnning minion");
            int randomNum = Random.Range(0,100);
            if(randomNum <= npcChance){
                GameObject tempNPC = Instantiate(brainWashed); //Create minon that fights for the player
                tempNPC.transform.position = transform.position; //Set the spawned enemies position to the currently killed enemies position;
                tempNPC.GetComponent<NPC>().health = NPChealth;
            }
            
        }
        // } else{
        //     Debug.Log("Tranced: " + tranced + "Hitting the puddle: " + hittingPuddle);
        // }
    }
    
    public void Poisoned(float durration, float damage, bool radioactive){
        if(!poisoned){
            GetPTime();
            pDurration = durration;
            poisoned = true;   
            pDamage = damage;
            radioactiveBool = radioactive;
            objectRend.color = Color.green;    
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
            objectRend.color = startColor; //if the enemy is poisoned, aka green, then don't make them change colour.
        }
        knockBack = false;
    }

}


//poison max how it will work:
//When the enemy enters a trigger that has the tag enemy, if it's "radioactive" bool is true, then if the enemy is in that trigger for say, 5 seconds, it will make this enemy radioactive,
//radiactive won't be applied to enemies until an enemy is hit by a player who has poisoned maxed out.
//This way I can avoid triggering a bool or some kind of method on every single enemy once the player gains the maxed ability.