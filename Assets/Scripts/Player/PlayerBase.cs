using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

//manages base player behaviour
public class PlayerBase : MonoBehaviour
{

    public float resist;
    public string className;
    public GameObject weaponObject;
    private GameObject weapon;
    public string ability;
    public float speed = 5.0f;
    public float health;
    private bool dead = false;
    private GameObject UI;
     
    void Start(){
        resist = 1f;
        UI = GameObject.Find("Canvas");
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
    public void takeDamage(float damage){
        if(!dead){
            //if not dead than display the hitmarker lower health and call died if health has hit or passed 0
            UI.GetComponent<UIManager>().DisplayHit((damage * resist),this.gameObject);
            health -= damage * resist;
            if(health <= 0){
                Died();
            }
        }
    }
    void Died(){
        //call dead
        dead = true;
        //visual queue showing dead until further UI changes.
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
