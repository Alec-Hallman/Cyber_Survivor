using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Meele base code deals damage when touching player game object.
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
            if((timer >= attackSpeed) && !hacked){
                player.GetComponent<PlayerBase>().takeDamage(damage);
                GetCurrentTime();
                //Debug.Log("Player Taking Damage");
            }
            
        }
    }

}
