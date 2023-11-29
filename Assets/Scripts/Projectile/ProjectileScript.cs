using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

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
        float counter = Time.realtimeSinceStartup - timer;
        Debug.Log("Counter: " + counter + "Travel Time: " + travelTime);
        if(counter < travelTime){
            //Debug.Log("My Position: " +transform.position + "targetPosition: " + currentPosition);
            GetComponent<Rigidbody2D>().velocity = direction * speed;
        }else{
            Debug.Log("Stio");
            GetComponent<Rigidbody2D>().velocity = zero;
        }
        float counter2 = Time.realtimeSinceStartup - timer2;
        if(counter2 >= lifeSpan){
            Destroy(this.gameObject);
        }
        
    }
    void OnTriggerEnter2D(Collider2D hit){
        if(hit.gameObject.tag == "Player"){
            hit.gameObject.GetComponent<PlayerBase>().takeDamage(damage);
            Destroy(this.gameObject);
        }
    }
    void GetCurrentTime(){
        timer = Time.realtimeSinceStartup;
    } 
}
