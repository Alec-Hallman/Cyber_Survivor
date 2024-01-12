using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class EnemySwarm : EnemyBase
{
    // Start is called before the first frame update
    public float lifeTime;
    private float timer;
    void Start()
    {
        timer = Time.realtimeSinceStartup;
        base.walking = true;
        EnemyStart();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMoveUpdate();
        if(hittingPlayer){
            float timer = Time.realtimeSinceStartup - time;
            if(timer >= attackSpeed){
                player.GetComponent<PlayerBase>().takeDamage(damage);
                GetCurrentTime();
                //Debug.Log("Player Taking Damage");
            }
            
        }
        if(Time.realtimeSinceStartup - timer > lifeTime){
            Destroy(this.gameObject);
        }
        
    }
}
