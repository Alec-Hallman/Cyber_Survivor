using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeeleBase : WeaponBase
{
    protected HashSet<GameObject>enemiesInRange;
    protected HashSet<GameObject>enemiesToRemove;
    // Start is called before the first frame update
    public void MeeleStart()
    {
         enemiesInRange = new HashSet<GameObject>();
        enemiesToRemove = new HashSet<GameObject>();
        WeaponStart();
    }

    // Update is called once per frame
    public void DealDamage(){
        foreach(GameObject enemy in enemiesInRange){
            if(enemy!= null){
                EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                if(enemyScript.health - damage <= 0){
                    enemyScript.takeDamage(damage, false,true);
                    enemiesToRemove.Add(enemy);
                    
                }else{
                    enemyScript.takeDamage(damage, false, true);
                }
                if(poison){
                    enemyScript.Poisoned(pDurration,pDamage,radioactive);
                }
                LifeSteal();
            }
        }
        if(dealDamage){
            Invoke("DealDamage", attackSpeed);
        }
        PurgeEnemiesList();
    }
    public void PurgeEnemiesList(){
        foreach(GameObject enemy in enemiesToRemove){
            if(enemy!=null){
                float distance = Vector2.Distance(this.transform.position, enemy.transform.position);
                Debug.Log(distance);
                if(distance > (maxDistance + radius)){ 
                    //Sometimes an enemy will walk into an object trigger and then walk out, get added to the remove list, but then walks back into the list but they'll still
                    //be removed as the game doesn't know they've walked back in, so I'm checking their distance ensuring they're actually out of range before removing them.
                    enemiesInRange.Remove(enemy);
                }
            }
        }
        enemiesToRemove.Clear();
    }
}
