using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private List<GameObject> inRange = new List<GameObject>();
    private List<GameObject> toRemove = new List<GameObject>();
    private bool hit2 = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CircleCollider2D>().radius = radius;
    }

    // Update is called once per frame
    void Update()
    {
        if(dealDamage){
            if(hit1 && Time.realtimeSinceStartup - time >= attackSpeed){
                DealDamage();
                Timer();
                hit1 = false;
                hit2 = true;
                return;
            }
            else if(hit2 && Time.realtimeSinceStartup - time >= attackInterval){
                DealDamage();
                Timer();
                hit2 = false;
                hit1 = true;
                return;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        //Debug.Log(collider.name);
        if(collider.gameObject.tag == "Enemy"){
            if(time == 0F){
                Timer();
            }
            if(!collider.isTrigger){
                inRange.Add(collider.gameObject);
            }
            //Debug.Log(inRange.ToString());
            dealDamage = true;
            enemyObject = collider;

            //Debug.Log("Hit enemy called deal Damage");
        }
    }
    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Enemy"){
            //
            if(!collider.isTrigger){
                toRemove.Add(collider.gameObject);
            }
            if(inRange.Count == 0){
                dealDamage = false;
            }
        }
    }
    void Timer(){
        time = Time.realtimeSinceStartup;
    }
    void DealDamage(){
        foreach(GameObject listObject in inRange){
            if(listObject != null){
                //If statement to see if the damage about to be delt to object will be fatal, if it will be remove it from the list atleast thats the idea.
                if((listObject.GetComponent<EnemyBase>().health - damage) <= 0){
                    //add it to a list to remove after this list is ran to avoid C# errors
                    toRemove.Add(listObject);
                    //inRange.Remove(listObject);
                }
                listObject.gameObject.GetComponent<EnemyBase>().takeDamage(damage);
                
            }
        }
        foreach(GameObject removeObjects in toRemove){
            //Remove items that need to be removed
            inRange.Remove(removeObjects);
        }
       
    }
}


//Weapon calls a take damage comand in the enemy object
//But if there are multiple enemies than the enemy will need to be added to a list
//It might be better to have the player controller contain this list, then again all items will have their own ranges so the list wont be acurate, unless there are multiple lists for each weapon.
//But that sounds horrible.
