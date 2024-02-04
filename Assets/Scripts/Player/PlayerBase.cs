using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

//manages base player behaviour
public class PlayerBase : MonoBehaviour
{
    [SerializeField]private SpriteRenderer playerrenderer;
    public float resist;
    private HealthBar healthBar;
    public string className;
    public GameObject weaponObject;
    private GameObject weapon;
    public string ability;
    public float speed = 5.0f;
    public float maxHealth;
    public float health;
    private bool dead = false;
    private GameObject UI;
    public bool paused = false;
    private Animator animator;
    public float projectileDodgeFactor;
    private bool animating;
    private bool dodged;
    private UIManager uiScript;
    private bool walking;
    private float walkTimer;
    private float standTimer;
    private SpawnScript spawnScript;
    public float phaseDurration = 0;
    public bool passThrough = false;
    private bool phasing;
    private bool canPhase;
    public float PhaseCooldown;
    private AbilityManager managerScript;
     
    void Start(){
        className = PlayerPrefs.GetString("Class");
        canPhase = true;
        spawnScript = GameObject.Find("EnemyManager").GetComponent<SpawnScript>();
        dodged = false;
        healthBar = GameObject.Find("Health").GetComponent<HealthBar>();
        animator = gameObject.GetComponent<Animator>();
        animating = false;
        health = maxHealth;
        resist = 1f;
        UI = GameObject.Find("Canvas");
        uiScript = UI.GetComponent<UIManager>();
        Invoke("initClassAbility",0.01f);
        managerScript = GameObject.Find("Manager").GetComponent<AbilityManager>();
        


        //Debug.Log(classInfo.name);
    }
    void Update()
    {
        //Movement Block
        Vector2 movement = new Vector2();

        if (Input.GetKey(KeyCode.W))
        {
            movement.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement.y = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1;
        }
        if(walking && movement.y == 0 && movement.x == 0){
            animator.SetBool("Walking", false);
            walking = false;
            GetStandTime();
        } if(movement.y != 0 || movement.x != 0){
            if (movement.x > 0){
                playerrenderer.flipX = true;
            }
            else if (movement.x < 0)
            {
                playerrenderer.flipX = false;
            }
            
            walking = true;
            animator.SetBool("Walking", true);
        }
        if(!walking && Time.realtimeSinceStartup - standTimer > 10){ //If the player is standing still for more than 10 seconds to encourage them to move again.
            //spawnScript.StartWave("RangedEnemyBullet", 1); //For one second only spawn ranged enemies
        }
        transform.Translate(movement * speed * Time.deltaTime);
        //End of movememnt block
        // if(Input.GetKeyDown(KeyCode.Delete)){
        //     Died();
        // }
    }
    public void takeDamage(float damage, bool projectile){
        if(projectile){
            float randomFloat = Random.Range(0.00f, 1.00f);
            if((randomFloat - projectileDodgeFactor) <= 0){
                dodged = true;
                //Debug.Log("Dodged");
                uiScript.DisplayImmune("Immune", this.gameObject);
            }
        }
        if(!dead && !(Time.timeScale == 0) && !dodged && !phasing){
            //if not dead than display the hitmarker lower health and call died if health has hit or passed 0
            //uiScript.DisplayHit((damage * resist),this.gameObject, false, false);
            healthBar.ReduceHealthBar(damage);
            health -= damage * resist;
            if(canPhase){
                phasing = true;

                if(passThrough){
                    gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                }
                //animator.SetBool("Phasing",true);
                Invoke("StopPhase", phaseDurration);
                canPhase = false;
                Invoke("CanPhase", PhaseCooldown);

            }
            if(health <= 0){
                Died();
            }
        } else if(dodged){
            dodged = false;
        }
        animator.SetBool("Damage",true);
        Invoke("StopDamage", 0.1f);
    }
    void Died(){
        //call dead
        dead = true;
        animator.SetBool("Dead", true);
        //visual queue showing dead until further UI changes.
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void GainHealth(float ammount){
        //Debug.Log("Gainning Health + : " +ammount);
        if(ammount > 0){
            //Debug.Log("Entering if statement");
            if(health < maxHealth && !dead){
                if(!animating){
                    //Debug.Log("Starting Animation");
                    animator.SetBool("Heal", true);
                    animating = true;
                    Invoke("StopHealAnimation", 0.15f); 
                }
                health += ammount;
                if(health > maxHealth){
                    health = maxHealth;
                }
                healthBar.IncreaseHealthBar(ammount);
                UI.GetComponent<UIManager>().DisplayHit(ammount,this.gameObject, true, false);
                //animator.SetBool("Heal", false);

            }
        }
    }
    private void StopDamage(){
        animator.SetBool("Damage", false);
    }
    private void StopHealAnimation(){
//        Debug.Log("Turning off animation");
        animator.SetBool("Heal", false);
        animating = false;
    }
    void GetWalkTime(){
        walkTimer = Time.realtimeSinceStartup;
    }
    void GetStandTime(){
        standTimer = Time.realtimeSinceStartup;
    }
    void StopPhase(){
        phasing = false;
        //animator.SetBool("Phasing",false);
        if(passThrough){
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        }
    }
    void CanPhase(){
        canPhase = true;
    }

    void initClassAbility(){
        //managerScript.ApplyClassCard(className);
        //Debug.Log("Name: " + className)
        string fileLocation = "Jsons/Classes/" + className;
        TextAsset jsonFile = Resources.Load<TextAsset>(fileLocation);
        //Debug.Log("FileLocation: " + fileLocation);
        string json = jsonFile.text;
        Classes classInfo = JsonUtility.FromJson<Classes>(json);
        className = classInfo.name;
        speed = classInfo.speed;
        ability = classInfo.ability;
        string weaponName = classInfo.weapon;
        AbilityManager tempScript = GameObject.Find("Manager").GetComponent<AbilityManager>();
        AbilityCards tempCard = tempScript.ApplyClassCard(ability);
        AbilityCards tempWeapon = tempScript.ApplyClassCard(weaponName);
        PlayerAbilitys tempScriptAbilities = gameObject.GetComponent<PlayerAbilitys>();
        tempScriptAbilities.ApplyAbility(tempCard);
        tempScriptAbilities.ApplyAbility(tempWeapon);

        //Debug.Log("Assets/Weapons/"+ classInfo.weapon +".prefab");
    }
}
