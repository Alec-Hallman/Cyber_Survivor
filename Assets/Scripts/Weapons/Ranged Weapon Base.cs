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
    [SerializeField] private GameObject nullTargetObject;
    private Transform nullTransform;
    private bool right;
    private bool spawned;
    protected Queue<GameObject>enemiesInRange;
    private Animator animator;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        StartProjectiles();
        enemiesInRange = new Queue<GameObject>();
        nullTransform = nullTargetObject.transform;
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
        if(hit.gameObject.tag.Contains("Enemy")){
            enemiesInRange.Enqueue(hit.gameObject);
            //Debug.Log("Projectile Script hit enemy");
            //enemyLocation = hit.gameObject.transform;
            // for(int i = 0; i < projectileCount; i++){
            //     Invoke("SpawnProjectiles", attackSpeed);
            //     Invoke("Done", attackSpeed);
            //     spawned = true;
            //     //bulletCounter += 0.1f;
            // }
            //bulletCounter = 0;
        }
    }
    private void Done(){
        spawned = false;
        //bulletCounter = 0;
    }
    private void ResetCounter(){
        bulletCounter = 0;
    }
    void StartProjectiles(){
        if(!spawned){
            for(int i = 0; i < projectileCount; i++){
                    Invoke("SpawnProjectiles", attackSpeed);
                    Invoke("Done", attackSpeed);
                    spawned = true;
                    //bulletCounter += 0.1f;
            }
            spawned = true;
            Invoke("ResetCounter", attackSpeed + 0.05f);
            if(shotCounter < magazineSize){
                Invoke("StartProjectiles",attackSpeed);
                if(!noReload){
                    shotCounter+=1;
                }
            } else{
                shotCounter = 0;
                Invoke("StartProjectiles", attackSpeed + reloadSpeed);
                Invoke("StartAnimation", attackSpeed + reloadSpeed);
                animator.SetBool("Reloading",true);
            }
        }
    }
    void StopAnimation(){
        animator.SetBool("Reloading",false);
    }
    void StartAnimation(){
        animator.SetBool("Reloading",true);
        Invoke("StopAnimation",0.5f);
    }
    void SpawnProjectiles(){
        //if(enemyLocation != null){ // if we have a enemy location
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
        if(enemyLocation != null){ //If we have an enemy location
            tempScript.targetLocation = enemyLocation;
        } else if(enemiesInRange.Count > 0){ //else if the queue isnt empty
            GameObject tempEnemy = QueueClean(); //try and find a new enemy
            if(tempEnemy != null){ //if an enemy is found
                enemyLocation = tempEnemy.transform;
                tempScript.targetLocation = enemyLocation;
            } else{ //if we dont find an enemy
                tempScript.targetLocation = nullTransform;
            }
        } else{ //The queue is empty;
            tempScript.targetLocation = nullTransform;
        }
        tempScript.parentScript = gameObject.GetComponent<WeaponBase>();
        tempScript.SendBack(false);
        tempScript.damage = damage;
        //Debug.Log("Projectile damage: " + tempScript.damage);
        // if(shotCounter < magazineSize && enemyLocation != null){
        //     Invoke("SpawnProjectiles", 0);
        // } else if (enemyLocation == null){
        //     Invoke("SpawnProjectiles", 0);
        //     shotCounter = 0;
        // }
       // }
    }
    private GameObject QueueClean(){
        GameObject enemy = enemiesInRange.Dequeue();
        while(enemy == null && enemiesInRange.Count > 0){
            enemy = enemiesInRange.Dequeue();
            float distance;
            if(enemy != null){
                distance = GetDistance(enemy); //Get the distance of how far away enemy is
                if(distance > maxDistance){ //if it's distance is greater than how far away it should be
                    enemy = null; //Remove it from the queue.
                }
            }
        }
        return enemy;
    }
}
