using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform targetLocation;
    private Transform staticTargetLocation;
    public float damage;
    private Vector2 direction;
    private Vector3 currentPosition;
    private float timer;

    private float travelTime;
    public float speed;
    private Vector2 zero = new Vector2(0,0);
    void Start()
    {
       
        currentPosition = targetLocation.position;
        travelTime = Random.Range(0.5F, 1.5F);
        GetCurrentTime();
         direction = (currentPosition - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
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
