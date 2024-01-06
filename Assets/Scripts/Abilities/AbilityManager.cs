using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AbilityManager : MonoBehaviour
{
    public AbilityCards[] abilityCards;
    public List<AbilityCards> activeCards;
    private GameObject levelUI;
    public List<AbilityCards> chosenCards;
    private GameObject card1;
    private GameObject card2;
    private GameObject card3;
    private Color cardColor;
    private int selected = 0;
    private bool uiOn;


    void Start(){
        initializeCards();
        uiOn = false;
        levelUI = GameObject.Find("LevelUpScreen");
        activeCards = new List<AbilityCards>();
        chosenCards = new List<AbilityCards>();

      //  levelUI.SetActive(true);
        card1 = GameObject.Find("Card1");
        card2 = GameObject.Find("Card2");
        card3 = GameObject.Find("Card3");
       cardColor = card1.GetComponent<Image>().color;
        levelUI.SetActive(false);

    }
    void Update(){
        if(uiOn){
            if(Input.GetKeyDown(KeyCode.Return)){
                 
                if(!activeCards.Contains(chosenCards[selected])){
                    AddAbility(chosenCards[selected]);
                }
                Debug.Log("Selected: "+selected);
                uiOn = false;
                levelUI.SetActive(false);
                Time.timeScale = 1; 
                //activeCards.Add(chosenCards[selected]);
                //Debug.Log("Selected Cards = " + activeCards.ToString());
                
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

    public void GenerateDisplay(){
        Time.timeScale = 0; //(This line pauses the game)
        chosenCards.Clear();
        while(chosenCards.Count < 3){
            int randomIndex = Random.Range(0,abilityCards.Length);
            if(!chosenCards.Contains(abilityCards[randomIndex])){
                //chosenCards.Add(abilityCards[randomIndex]);
                if(activeCards.Contains(abilityCards[randomIndex])){
                    abilityCards[randomIndex].UpTier();
                   
                }
                chosenCards.Add(abilityCards[randomIndex]);
                

            }
        }
        card1.GetComponent<AbilityCardDisaply>().card = chosenCards[0];
        card2.GetComponent<AbilityCardDisaply>().card = chosenCards[1];
        card3.GetComponent<AbilityCardDisaply>().card = chosenCards[2];
        Print();
        uiOn = true;
        levelUI.SetActive(true);
    }
    private void Print(){
        string output = "Cards generated: ";
        foreach(AbilityCards card in chosenCards ){
            output = output + card.name + " ";
        }
        Debug.Log(output);
    }
    private void AddAbility(AbilityCards ability){
        activeCards.Add(ability);
    }
    void initializeCards(){
        foreach(AbilityCards card in abilityCards){
            card.initCard();
        }
    }
    
}
