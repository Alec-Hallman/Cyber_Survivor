using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public float speed = 5.0f;
    public float health;
    private bool dead = false;
    private GameObject UI;
    void Start(){
        UI = GameObject.Find("Canvas");
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
        if(Input.GetKeyDown(KeyCode.Delete)){
            Died();
        }
    }
    public void takeDamage(float damage){
        if(!dead){
            UI.GetComponent<UIManager>().DisplayHit(damage,this.gameObject);
            health -= damage;
            if(health <= 0){
                Died();
            }
        }
    }
    void Died(){
        dead = true;
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
