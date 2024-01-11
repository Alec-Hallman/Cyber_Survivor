using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Enemy ranged script that spawns projectiles and only moves until in shooting range, instead of toutching the enemy.
public class EnemyRangedBase : EnemyBase
{
    // Start is called before the first frame update
    public GameObject projectile;
    void Start()
    {
        base.EnemyStart();
    }

    // Update is called once per frame
    void Update()
    {
        base.EnemyMoveUpdate();
        if(hittingPlayer){
            float timer = Time.realtimeSinceStartup - time;
            base.walking = false;
            if(timer >= attackSpeed){
                GameObject bullet = Instantiate(projectile);
                bullet.transform.position = transform.position;
                bullet.GetComponent<ProjectileScript>().targetLocation = base.player.transform;
                bullet.GetComponent<ProjectileScript>().damage = damage;
                GetCurrentTime();
            }
            
        } else if (base.walking == false){
            base.walking = true;
        }
    }
}
