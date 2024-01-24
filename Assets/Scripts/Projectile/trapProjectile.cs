using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapProjectile : ProjectileScript
{
    // Start is called before the first frame update
    private SpriteRenderer myRenderer;
    private float halfTravel;
    private float currentXSize;
    private float currentYSize;
    private float startXSize;
    private float startYSize;
    private float timeTracker;
    private ParticleSystem particles;
    //Anmimation time refers to the time needed to run particle system
    public float animationTime;
    private float animationTimeTracker;
    private float currentTime;
    void Start()
    {
        base.BaseProjectileStart();
        explosive = true;
        particles = gameObject.GetComponent<ParticleSystem>();
        particles.Stop();
        myRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        myRenderer.sortingOrder = 5;
        currentYSize = transform.localScale.y;
        currentXSize = transform.localScale.x;
        startXSize = currentXSize;
        startYSize = currentYSize;
        timeTracker = Time.realtimeSinceStartup;
        traveling = true;
        travelTime = Random.Range(0.5F, 1.5F);
        halfTravel = travelTime / 2;
        //Set travel time to be random instead of a set time.
    }
    //The idea with this script is to animate the trap/mine projectile. This just handels the special behaviour of this type of projectile.
    // Update is called once per frame
    void Update()
    {
        //Call the base update method
        base.BaseProjectileUpdate();
        if(base.explodeNow){
            
            if(!particles.isPlaying){
                animationTimeTracker = Time.realtimeSinceStartup;
                particles.Play();
            }
            Explode();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        //float for tracking time
        
    }
    void FixedUpdate(){
        float totalTime = Time.realtimeSinceStartup - timeTracker;
        //if the object is moving and the object has been around for half of the time it's traveling for
        if(traveling && totalTime < halfTravel){
            //Increase the scale by a damping float of 1.015F every frame
            if(currentXSize < 1.3f){
                currentXSize *= 1.05f;
                currentYSize *= 1.05f;
            } else{
                currentXSize *= 1.01f;
                currentYSize *= 1.01f;

            }
            transform.localScale = new Vector2(currentXSize,currentYSize);
            Debug.Log("Current x"  + currentXSize);

        }else if (traveling){
            //If we've past the half way mark for travel time than decrease the size, making sure it can't be smaller than the starting size 
            if(currentXSize > startXSize){
                currentXSize *= 0.95F;
            }
            if(currentYSize > startYSize){
                currentYSize *= 0.95F;
            }
            //Apply the vector 2 changes
            transform.localScale = new Vector2(currentXSize,currentYSize);
        }

        //If the object has stopped traveling than make it render under the player
        if(!base.traveling){
            myRenderer.sortingOrder = 0;
        }
    }
    void Explode(){
        //Debug.Log("Called");

        float newCounter = Time.realtimeSinceStartup - animationTimeTracker;
        if(newCounter >= animationTime){
             Destroy(this.gameObject);
        }
        
    }
}

