using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
//This script manages the hitmarker text creation

public class UIManager : MonoBehaviour
{
    //This code should be attached to the canvas
    // Start is called before the first frame update
    public GameObject text;
    private Vector2 size;
    private  GameObject textClone = null;
    private GameObject objectPointer = null;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DisplayHit(float number, GameObject givenObject, bool gain, bool poison){
        //Note Number = the damage
        objectPointer = givenObject;
        number = (float)Math.Round(number, 1);
        textClone = Instantiate(text, transform);
        textClone.GetComponent<TextMeshProUGUI>().text = number.ToString();
        textClone.GetComponent<DamageNumber>().getObject = givenObject;
        float scale = 0F;
        if(givenObject.tag == "Enemy"){
            //If an enemy is taking damage set the size to always be 0.8F
            scale = 0.8F;
            //Unless It's poisoned because it gets hard to see things with all the big markers.
            if(poison){
                scale = 0.4f;
            }
        }
        else if(number > 30){
            scale = 2;
            //this is the max size of hit marker
            
        } else if(number > 15){
            scale = 1.2f;
        }
        else{
            scale = 0.8f;
        }
        //apply scale changes
        if(gain){
            scale = 0.3f;
        }
        size = new Vector2(scale,scale);
        textClone.transform.localScale = size;

        if(givenObject.tag == "Player"){
            //if the object hit is the player than the text colour should be red, makes it easier to read
            if(!gain){
                textClone.GetComponent<TextMeshProUGUI>().color = Color.red;
            } else if (poison){
                textClone.GetComponent<TextMeshProUGUI>().color = Color.yellow;
            }else{
                textClone.GetComponent<TextMeshProUGUI>().color = Color.green;
            }
        }
        else if(givenObject.tag == "Enemy"){
            //if the object is an enemy make the colour black.
            if(!gain){
                textClone.GetComponent<TextMeshProUGUI>().color = Color.black;
            } else if (poison){
                textClone.GetComponent<TextMeshProUGUI>().color = Color.yellow;
            }else{
                textClone.GetComponent<TextMeshProUGUI>().color = Color.green;
            }
        }

    }
    public void DisplayImmune(string message, GameObject givenObject){
        textClone = Instantiate(text, transform);
        textClone.GetComponent<TextMeshProUGUI>().text = message;
        textClone.GetComponent<TextMeshProUGUI>().color = Color.grey;
        textClone.GetComponent<TextMeshProUGUI>().fontSize = 28f;
        textClone.GetComponent<DamageNumber>().getObject = givenObject;
        float scale = 1f;
        size = new Vector2(scale,scale);
        textClone.transform.localScale = size;
    }
}
