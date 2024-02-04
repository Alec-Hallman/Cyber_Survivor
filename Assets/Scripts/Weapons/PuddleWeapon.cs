using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleWeapon : WeaponMeeleBase
{
    // Start is called before the first frame update
    public float maxSize;
    private float timer;
    public float lifeTime;
    void Start()
    {
        timer = Time.realtimeSinceStartup;
        MeeleStart();
        this.transform.localScale = new Vector2(maxSize, maxSize);
        gameObject.GetComponent<CircleCollider2D>().radius = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.realtimeSinceStartup - timer > lifeTime){
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D hit){
        if(hit.gameObject.tag.Contains("Enemy")){
            if(enemiesInRange.Count == 0){
                dealDamage = true;
                Invoke("DealDamage",attackSpeed);
            }
            enemiesInRange.Add(hit.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D hit){
        if(hit.gameObject.tag.Contains("Enemy")){
            if(enemiesInRange.Contains(hit.gameObject)){
                enemiesToRemove.Add(hit.gameObject);

            }
        }
    }
}
