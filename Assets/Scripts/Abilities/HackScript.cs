using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackScript : MonoBehaviour
{
    public float frequency;
    public float radius;
    private Vector2 targetTransform;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        targetTransform = transform.localScale * (radius * 8);
        speed = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, targetTransform, (speed *= 1.05f));
        //Debug.Log("Current transform: " + transform.localScale + " Target Transform: " + targetTransform);
        if((Vector2)transform.localScale == targetTransform){
           // Debug.Log("Done Lerping");
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Enemy"){
            //Debug.Log("hacking enemy");
        }
}    
}
