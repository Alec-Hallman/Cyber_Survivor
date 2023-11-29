using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    // Start is called before the first frame update
    private float timer;
    public float duration;
    private float counter;
    private Vector2 startPosition;
    private Vector2 endPosition;
    public GameObject getObject;
    void Start()
    {
        timer = Time.realtimeSinceStartup;
        startPosition = getObject.transform.position;
        transform.position = startPosition;
        endPosition = startPosition + Random.insideUnitCircle;
    }

    // Update is called once per frame
    void Update()
    {   
        float t = counter / duration;
        transform.position = Vector2.Lerp(startPosition,endPosition,t);
        counter = Time.realtimeSinceStartup - timer;
        if(counter >= duration){
            Destroy(this.gameObject);
        }
        
    }
}
