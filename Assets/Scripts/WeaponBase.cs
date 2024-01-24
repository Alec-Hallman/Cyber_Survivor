using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//Player Weapon base behaviour
public class WeaponBase : MonoBehaviour
{
    private bool dealDamage = false;
    public float damage;
    public float attackSpeed;
    public float attackInterval;
    public float radius;
    private float time = 0F;
    private Collider2D enemyObject;
    private bool hit1 = true;
    private HashSet<GameObject> inRange = new HashSet<GameObject>();
    private HashSet<GameObject> toRemove = new HashSet<GameObject>();
    private bool hit2 = false;
    private Animator animator;
    private bool damaging;
    private GameObject player;
    private float speedHolder;
    public float pDamage;
    public float pDurration;
    public bool swinging;
    public bool poison;
    public bool radioactive;
    public bool deflect;
    public float steal = 0;
    // Start is called before the first frame update
    void Start()
    {
        attackSpeed = 1.5f;
        damaging = false;
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        GetComponent<CircleCollider2D>().radius = radius;
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange.Count == 0){
                dealDamage = false;
                hit1 = true;
                hit2 = false;
                animator.SetBool("R-L", false);
                animator.SetBool("L-R", false);
        }
        if(dealDamage && !(Time.timeScale == 0)){
            //RemoveFromList();
            //if damage is to be delt
            if(hit1 && Time.realtimeSinceStartup - time >= attackSpeed){
                DealDamage(true);
                swinging = true;
                Invoke("NotSwinging",0.1f);
                //Debug.Log("Hit1");
                animator.SetBool("R-L", false);
                animator.SetBool("L-R", true);
                Timer();
                hit1 = false;
                hit2 = true;
            }
            else if(hit2 && Time.realtimeSinceStartup - time >= attackInterval){
                swinging = true;
                Invoke("NotSwinging",0.2f);
                DealDamage(true);
                //Debug.Log("Hit2");
                animator.SetBool("L-R", false);
                animator.SetBool("R-L", true);
                Timer();
                hit2 = false;
                hit1 = true;
            }
            //These hit 1 and 2 checks allow the attacks to hit on a 1-2 beat, this can be negated by making attack interval and attack speed the same value.
        }
    }
    void OnTriggerStay2D(Collider2D collider){
        //Debug.Log(collider.name);
        if(collider.gameObject.tag == "Enemy" && !damaging){
            if(time == 0F){
                Timer();
            }
            if(!collider.isTrigger && !inRange.Contains(collider.gameObject)){
                inRange.Add(collider.gameObject); // add game object to list of objects to deal damage to
            }
            //Debug.Log(inRange.ToString());
            dealDamage = true;
            //animator.SetBool("DealDamage", true);
            enemyObject = collider;

            //Debug.Log("Hit enemy called deal Damage");
        } else if( deflect && collider.gameObject.tag == "Going"){
            if(time == 0f){
                Timer();
            }
            if(inRange.Count == 0){
                inRange.Add(collider.gameObject);
                dealDamage = true;
                speedHolder = attackSpeed;
                attackSpeed = 0.2f;
                Invoke("CleanInRange", 0.4f);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Enemy"){
            //
            if(!collider.isTrigger){
                toRemove.Add(collider.gameObject);
            }

        }
    }
    void Timer(){
        time = Time.realtimeSinceStartup;
    }
    void DealDamage(bool remove){
        damaging = true;
        foreach(GameObject listObject in inRange){ //for each loop that deals damage to all objects in list
            if(listObject != null && listObject.tag == "Enemy"){
                EnemyBase enemyBase = listObject.GetComponent<EnemyBase>();
                //If statement to see if the damage about to be delt to object will be fatal, if it will be remove it from the list atleast thats the idea.
                if((enemyBase.health - damage) <= 0){
                    //add it to a list to remove after this list is ran to avoid C# errors
                    toRemove.Add(listObject);
                    //inRange.Remove(listObject);
                }
                listObject.gameObject.GetComponent<EnemyBase>().takeDamage(damage, false);
                if(poison){
                    listObject.gameObject.GetComponent<EnemyBase>().Poisoned(pDurration,pDamage,radioactive);
                }
                //Debug.Log("Gain Health, Damage = " + damage + " Life steal ammount = " + damage*steal + "Steal = " + steal);
                player.GetComponent<PlayerBase>().GainHealth(damage * steal);
            }
        }
        if(remove){
            RemoveFromList();
        }
        damaging = false;
    }
    void RemoveFromList(){
        foreach(GameObject removeObjects in toRemove){
                //Remove items that need to be removed
                inRange.Remove(removeObjects);

        }
        toRemove.Clear();
    }
    private void NotSwinging(){
        swinging = false;
    }
    void CleanInRange(){
        attackSpeed = speedHolder;
        inRange.Clear();
    }
}

//Idea:
//Weapon calls a take damage comand in the enemy object
//So each enemy becomes responsible for it's own damage taken, making it track if it walks into weapon range
//But this creates an issue, suddenly the enemy becomes responsible for traking attack time, which is super doable but the enemies would'nt take damage syncronusly. Unless the timer is delt with
//In some kind of controller script. That can be publicly accessed, but this sounds more complicated. More efficient? Probobly, as it avoids the list. Overall this was simpliler and keeps the 
//code in one chunk, so I've opted for this.