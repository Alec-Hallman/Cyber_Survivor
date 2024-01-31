using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

//This script handels basic projectile behaviour. All projectiles will be chasing the gameobject given to them and travel to that location. They will also deal damage.

public class ProjectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public Transform targetLocation;
    public bool poison;
    public float pDamage;
    public float pDurration;
    public bool radioactive;
    public LayerMask projectileLayer;
    private Transform staticTargetLocation;
    private PlayerBase playerScript;
    public WeaponBase parentScript;
    public float damage;
    private Vector2 direction;
    private Vector3 currentPosition;
    public float lifeSpan;
    private float timer;
    public float travelTime;
    public float speed;
    [HideInInspector]
    public bool traveling;
    private float timer2;

    [HideInInspector]
    public bool explosive;
    private Vector2 zero = new Vector2(0,0);

    [HideInInspector]
    public bool explodeNow = false;

    public void BaseProjectileStart()
    {
        explosive = false;
        timer2 = Time.realtimeSinceStartup;
        currentPosition = targetLocation.position;
        GetCurrentTime();
        direction = (currentPosition - transform.position).normalized;
    }

    // Update is called once per frame
    public void BaseProjectileUpdate()
    {
        //Counter tracks the time since timer has been called
        float counter = Time.realtimeSinceStartup - timer;
        //Debug.Log("Counter: " + counter + "Travel Time: " + travelTime);
        //If the counter hasn't exceeded the travelTime limit value, then move the projectile
        if(counter < travelTime){
            //Debug.Log("My Position: " +transform.position + "targetPosition: " + currentPosition);
            GetComponent<Rigidbody2D>().velocity = direction * speed;
        }else{
            //Debug.Log("Stio");
            //if the projectile has stopped moving set the traveling bool to false (so that damage can be done) and velocity is set to zero (to stop moving the object)
            traveling = false;

            GetComponent<Rigidbody2D>().velocity = zero;
        }
        //Another counter for tracking time
        float counter2 = Time.realtimeSinceStartup - timer2;
        if(counter2 >= lifeSpan){
            //if the current time is at the lifeSpan limit then destroy the game object
            if(!explosive){
                Destroy(this.gameObject);
            }
            else{
                explodeNow = true;
            }
        }
    }
    //This trigger deals with damage dealing.
    void OnTriggerEnter2D(Collider2D hit){
       // Debug.Log("Hit something");
        //if it hits the player
        if( !traveling && hit.gameObject.tag == "Player"){
            //Then get the player base component and call the take damage method, passing the damage of this enemy.
            
            playerScript = hit.gameObject.GetComponent<PlayerBase>();
            playerScript.takeDamage(damage, true);
            //Destroy the projectile
            if(!explosive){
                Destroy(this.gameObject);
            }
            else{
                explodeNow = true;
            }
        }
        if(hit.gameObject.tag == "Weapon" && gameObject.tag == "Going"){
            WeaponBase tempScript = hit.GetComponent<WeaponBase>();
            if(tempScript.deflect && tempScript.swinging){
                //Debug.Log("hitWeapon");
                SendBack(true);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D hit){
         if(hit.gameObject.tag.Contains("Enemy") && gameObject.tag == "SendBack"){
            EnemyBase tempScript = hit.gameObject.GetComponent<EnemyBase>();
            tempScript.takeDamage(damage, false);
            if(parentScript != null){
                parentScript.LifeSteal();
            }
            if(poison){
                tempScript.Poisoned(pDurration, pDamage,radioactive);
            }
            if(!explosive){
                Destroy(this.gameObject);
            }
            else{
                explodeNow = true;
            }
        }
    }
    void GetCurrentTime(){
        timer = Time.realtimeSinceStartup;
    } 
    public void SendBack(bool deflected){
        if(deflected){
            direction *= -1;
        }
        gameObject.tag = "SendBack";
        gameObject.layer = 9; //Set the 
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        //Debug.Log(gameObject.layer);
        gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
    }
}
