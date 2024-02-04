using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//Player Weapon base behaviour
public class WeaponBase : MonoBehaviour
{
    public float damage;
    protected bool dealDamage;
    public float magazineSize;
    public float attackSpeed;
    public float attackInterval;
    public float projectileCount;
    public float radius;
    [SerializeField] float maxDistance;
    public GameObject player;
    public float pDamage;
    public float pDurration;
    public bool swinging;
    public bool poison;
    public bool radioactive;
    public bool deflect;
    public float steal = 0;
    public bool noReload;
    public float size;
    protected HashSet<GameObject>enemiesInRange;
    protected HashSet<GameObject>enemiesToRemove;
    // Start is called before the first frame update
    public void WeaponStart()
    {
        enemiesInRange = new HashSet<GameObject>();
        enemiesToRemove = new HashSet<GameObject>();
        player = GameObject.Find("Player");
        GetComponent<CircleCollider2D>().radius = radius;
    }
    public void LifeSteal(){
//        Debug.Log("Calling Steal Steal value" + steal);
        player.GetComponent<PlayerBase>().GainHealth(damage * steal);
    }
    public void DealDamage(){
        foreach(GameObject enemy in enemiesInRange){
            if(enemy!= null){
                EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                if(enemyScript.health - damage <= 0){
                    enemyScript.takeDamage(damage, poison);
                    enemiesToRemove.Add(enemy);
                    
                }else{
                    enemyScript.takeDamage(damage, false);
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