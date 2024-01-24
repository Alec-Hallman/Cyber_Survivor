using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Enemy ranged script that spawns projectiles and only moves until in shooting range, instead of toutching the enemy.
public class EnemyRangedBase : EnemyBase
{
    public int ammount;
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
        if(playerInRange && Time.timeScale != 0){
            float timer = Time.realtimeSinceStartup - time;
            base.walking = false;
            if(timer >= attackSpeed){
                float tempTimer = 0.01f;
                for(int i = 0; i < ammount; i++){
                    Invoke("Spawn", tempTimer);
                    tempTimer += 0.1f;
                }
                GetCurrentTime();
            }
            
        } else if (!playerInRange && base.walking == false){
            base.walking = true;
        }
        //Note to future me: sChange this code to stop using a trigger, this will simplify the base script and this script. Just use Vector3 distance
    }
    void Spawn(){
        if(Time.timeScale != 0){
            GameObject bullet = Instantiate(projectile);
            bullet.transform.position = transform.position;
            bullet.GetComponent<ProjectileScript>().targetLocation = base.player.transform;
            bullet.GetComponent<ProjectileScript>().damage = damage;
        }

    }
}
