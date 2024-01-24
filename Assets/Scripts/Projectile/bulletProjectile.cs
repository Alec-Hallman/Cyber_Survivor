using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletProjectile : ProjectileScript
{
    // Start is called before the first frame update
    private SpriteRenderer myRenderer;
    private ParticleSystem particles;
    //Anmimation time refers to the time needed to run particle system
    public float animationTime;
    private float animationTimeTracker;
    void Start()
    {
        base.BaseProjectileStart();
        explosive = true;
        //particles = gameObject.GetComponent<ParticleSystem>();
        //particles.Stop();
        myRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        myRenderer.sortingOrder = 5;
        traveling = false;
        //Set travel time to be random instead of a set time.
    }
    //The idea with this script is to animate the trap/mine projectile. This just handels the special behaviour of this type of projectile.
    // Update is called once per frame
    void Update()
    {
        //Call the base update method
        base.BaseProjectileUpdate();
        if(base.explodeNow){
            
            // if(!particles.isPlaying){
            //     animationTimeTracker = Time.realtimeSinceStartup;
            //     particles.Play();
            // }
            Explode();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        //float for tracking time
    }
    void Explode(){
        //Debug.Log("Called");

        float newCounter = Time.realtimeSinceStartup - animationTimeTracker;
        if(newCounter >= animationTime){
             Destroy(this.gameObject);
        }
        
    }
}

