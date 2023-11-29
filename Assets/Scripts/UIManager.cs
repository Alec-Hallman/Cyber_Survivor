using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    public void DisplayHit(float number, GameObject givenObject){
        //Note Number = the damage
        objectPointer = givenObject;
        textClone = Instantiate(text, transform);
        textClone.GetComponent<TextMeshProUGUI>().text = number.ToString();
        textClone.GetComponent<DamageNumber>().getObject = givenObject;
        float scale = 0F;
        if(givenObject.tag == "Enemy"){
            //If an enemy is taking damage set the size to always be 0.8F
            scale = 0.8F;
        }
        else if(number > 30){
            scale = 2;
            //this is the max size of hit marker
            
        } else if(number > 15){
            scale = 1.2F;
        }
        else{
            scale = (number/4);
        }
        //apply scale changes
        size = new Vector2(scale,scale);
        textClone.transform.localScale = size;

        if(givenObject.tag == "Player"){
            //if the object hit is the player than the text colour should be red, makes it easier to read
            textClone.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else if(givenObject.tag == "Enemy"){
            //if the object is an enemy make the colour black.
            textClone.GetComponent<TextMeshProUGUI>().color = Color.black;
        }

    }
}
