using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

//This script handels basic projectile behaviour. All projectiles will be chasing the gameobject given to them and travel to that location. They will also deal damage.

public class ProjectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform targetLocation;
    private Transform staticTargetLocation;
    public float damage;
    private Vector2 direction;
    private Vector3 currentPosition;
    public float lifeSpan;
    private float timer;
    public float travelTime;
    public float speed;
    public bool traveling;
    private float timer2;
    private Vector2 zero = new Vector2(0,0);
    public void BaseProjectileStart()
    {
        timer2 = Time.realtimeSinceStartup;
        currentPosition = targetLocation.position;
        travelTime = Random.Range(0.5F, 1.5F);
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
            Destroy(this.gameObject);
        }
    }
    //This trigger deals with damage dealing.
    void OnTriggerEnter2D(Collider2D hit){
        //if it hits the player
        if( !traveling && hit.gameObject.tag == "Player"){
            //Then get the player base component and call the take damage method, passing the damage of this enemy.
            hit.gameObject.GetComponent<PlayerBase>().takeDamage(damage);
            //Destroy the projectile
            Destroy(this.gameObject);
        }
    }
    void GetCurrentTime(){
        timer = Time.realtimeSinceStartup;
    } 
}
