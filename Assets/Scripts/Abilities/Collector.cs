using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    // Start is called before the first frame update
    private CircleCollider2D circle;
    private bool expand;
    void Start()
    {
        circle = gameObject.GetComponent<CircleCollider2D>();
        expand = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(expand){
            circle.radius += 10f;
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "XP" && expand){
            Debug.Log("FoundXp");
            col.gameObject.GetComponent<XP>().GoToPlayer();
        } else if(col.gameObject.name == "Player" && !expand){
            expand = true;
            Invoke("Delete", 2.5f);

        }
    }
    void Delete(){
        Destroy(this.gameObject);
    }
}
