using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeeleBase : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
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
    }

}
