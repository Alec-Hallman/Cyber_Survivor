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

    // Update is called once per frame
    void Update()
    {
        float totalTime = Time.realtimeSinceStartup - timeTracker;
        if(traveling && totalTime < halfTravel){
            transform.localScale = new Vector2((currentXSize *= 1.015F),(currentYSize *= 1.015F));
        }else if (base.traveling){
            if(currentXSize > startXSize){
                currentXSize *= 0.99F;
            }
            if(currentYSize > startYSize){
                currentYSize *= 0.99F;
            }
            transform.localScale = new Vector2(currentXSize,currentYSize);
        }
        BaseProjectileUpdate();
        if(!base.traveling){
            myRenderer.sortingOrder = 0;
        }
        //Projectile base Update is in charge of moving the projectile in the
        //direction of the player game object
        //It also deletes or stops the game object after x amount of time
    }
}
