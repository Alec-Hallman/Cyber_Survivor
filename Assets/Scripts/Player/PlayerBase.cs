using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

//manages base player behaviour
public class PlayerBase : MonoBehaviour
{

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
     
    void Start(){
        dodged = false;
        healthBar = GameObject.Find("Health").GetComponent<HealthBar>();
        animator = gameObject.GetComponent<Animator>();
        animating = false;
        health = maxHealth;
        resist = 1f;
        UI = GameObject.Find("Canvas");
        uiScript = UI.GetComponent<UIManager>();
        string json = File.ReadAllText("Assets/Jsons/Classes/Ninja.json");
        Calsses classInfo = JsonUtility.FromJson<Calsses>(json);
        className = classInfo.name;
        speed = classInfo.speed;
        ability = classInfo.ability;
        //Debug.Log("Assets/Weapons/"+ classInfo.weapon +".prefab");
        weaponObject = Resources.Load<GameObject>("Weapons/Katana");
        weapon = Instantiate(weaponObject);
        Vector3 scale = new Vector3(weapon.transform.localScale.x,(weapon.transform.localScale.y),0f);
        weapon.transform.parent = this.transform;
        weapon.transform.localScale = (scale + scale);
        weapon.transform.position = new Vector3 ((weapon.transform.position.x + 100f), weapon.transform.position.y, 0f);
        


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
        if(!dead && !(Time.timeScale == 0) && !dodged){
            //if not dead than display the hitmarker lower health and call died if health has hit or passed 0
            uiScript.DisplayHit((damage * resist),this.gameObject, false, false);
            healthBar.ReduceHealthBar(damage);
            health -= damage * resist;
            if(health <= 0){
                Died();
            }
        } else if(dodged){
            dodged = false;
        }
    }
    void Died(){
        //call dead
        dead = true;
        animator.SetBool("Dead", true);
        //visual queue showing dead until further UI changes.
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void GainHealth(float ammount){
        if(ammount > 0){
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
    private void StopHealAnimation(){
//        Debug.Log("Turning off animation");
        animator.SetBool("Heal", false);
        animating = false;
    }
}
