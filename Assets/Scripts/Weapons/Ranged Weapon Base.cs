using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedWeaponBase : WeaponBase
{
    // Start is called before the first frame update
    private float reloadSpeed = 0.5f;
    private Transform enemyLocation;
    public GameObject projectile;
    private int shotCounter;
    private float bulletCounter;
    private bool right;
    private bool spawned;
    void Start()
    {
        spawned = false;
        shotCounter = 0;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position;
    }
    void OnTriggerEnter2D(Collider2D hit){
        if(hit.gameObject.tag.Contains("Enemy") && enemyLocation == null && !spawned){
            //Debug.Log("Projectile Script hit enemy");
            enemyLocation = hit.gameObject.transform;
            for(int i = 0; i < projectileCount; i++){
                Invoke("SpawnProjectiles", attackSpeed);
                Invoke("Done", attackSpeed);
                spawned = true;
                //bulletCounter += 0.1f;
            }
            //bulletCounter = 0;
        }
        
    }
    void OnTriggerExit2D(Collider2D hit){
        if(enemyLocation != null && hit.gameObject.transform == enemyLocation){ //if there is an enemy location and the enemy that has walked out of range was the enemy we were tracking
            enemyLocation = null; // reset the enemy location
        }
        
    }
    private void Done(){
        spawned = false;
        //bulletCounter = 0;
    }
    void SpawnProjectiles(){
        if(enemyLocation != null){ // if we have a enemy location
            GameObject tempObj = Instantiate(projectile);
            tempObj.transform.localScale = new Vector2(tempObj.transform.localScale.x + 0.05f, tempObj.transform.localScale.y + 0.05f);
            tempObj.transform.position = transform.position + new Vector3(bulletCounter,0,0);
            if(right){
                bulletCounter += 0.001f;
                bulletCounter *= -1;
                right = false;
            } else if(!right){
                bulletCounter *= -1;
                right = true;

            }
            ProjectileScript tempScript = tempObj.GetComponent<ProjectileScript>();
            if(poison){ // if the poison ability is active on this weapon (this should be simplified into weapon base later on)
               // Debug.Log("Setting Poison Bullet");
                tempScript.poison = poison;
                tempScript.pDamage = pDamage;
                tempScript.pDurration = pDurration;
                tempScript.radioactive = radioactive;
            }
            tempScript.targetLocation = enemyLocation;
            tempScript.parentScript = gameObject.GetComponent<WeaponBase>();
            tempScript.SendBack(false);
            tempScript.damage = damage;
            //Debug.Log("Projectile damage: " + tempScript.damage);
            if(!noReload){
                shotCounter++;
            }
            if(shotCounter < magazineSize && enemyLocation != null){
                Invoke("SpawnProjectiles", attackSpeed);
            } else if (enemyLocation == null){
                Invoke("SpawnProjectiles", attackSpeed + reloadSpeed);
                shotCounter = 0;
            }
        }
    }
}
