using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    // Start is called before the first frame update
    private int selected;
    private GameObject card1;
    private GameObject card2;
    private GameObject card3;
    private Color cardColor;
    void Start()
    {
        card1 = GameObject.Find("Card1");
        card2 = GameObject.Find("Card2");
        card3 = GameObject.Find("Card3");
        cardColor = card1.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){
            if(selected == 0){}
            else if(selected == 1){

            }
            else if (selected == 2){

            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
                selected = (selected + 1) % 3;
               // Debug.Log("Selected: #" + selected);
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                selected = selected - 1;
                if(selected == -1){
                    selected = 2;
                }
               // Debug.Log("Selected: #" + selected);
            }
            if(selected == 0){
                card1.GetComponent<Image>().color = Color.blue;
                card2.GetComponent<Image>().color = cardColor;
                card3.GetComponent<Image>().color = cardColor;
            }
            else if(selected == 1){
                 card2.GetComponent<Image>().color = Color.blue;
                 card1.GetComponent<Image>().color = cardColor;
                 card3.GetComponent<Image>().color = cardColor;
            }
            else if(selected == 2){
                card3.GetComponent<Image>().color = Color.blue;
                card2.GetComponent<Image>().color = cardColor;
                card1.GetComponent<Image>().color = cardColor;
            }
    }
}
