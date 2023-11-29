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
    void Start()
    {
        myRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        myRenderer.sortingOrder = 5;
        currentYSize = transform.localScale.y;
        currentXSize = transform.localScale.x;
        startXSize = currentXSize;
        startYSize = currentYSize;
        timeTracker = Time.realtimeSinceStartup;
        traveling = true;
        BaseProjectileStart();
        travelTime = Random.Range(0.5F, 1.5F);
        halfTravel = travelTime / 2;
        //Set travel time to be random instead of a set time.
    }
    //The idea with this script is to animate the trap/mine projectile. This just handels the special behaviour of this type of projectile.
    // Update is called once per frame
    void Update()
    {
        //float for tracking time
        float totalTime = Time.realtimeSinceStartup - timeTracker;
        //if the object is moving and the object has been around for half of the time it's traveling for
        if(traveling && totalTime < halfTravel){
            //Increase the scale by a damping float of 1.015F every frame
            transform.localScale = new Vector2((currentXSize *= 1.015F),(currentYSize *= 1.015F));
        }else if (traveling){
            //If we've past the half way mark for travel time than decrease the size, making sure it can't be smaller than the starting size 
            if(currentXSize > startXSize){
                currentXSize *= 0.99F;
            }
            if(currentYSize > startYSize){
                currentYSize *= 0.99F;
            }
            //Apply the vector 2 changes
            transform.localScale = new Vector2(currentXSize,currentYSize);
        }
        //Call the base update method
        BaseProjectileUpdate();
        //If the object has stopped traveling than make it render under the player
        if(!base.traveling){
            myRenderer.sortingOrder = 0;
        }
    }
}
