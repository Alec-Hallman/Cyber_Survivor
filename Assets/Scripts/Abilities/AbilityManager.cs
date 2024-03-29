using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AbilityManager : MonoBehaviour
{
    private bool removable;
    public AbilityCards healthCard;
    public AbilityCards[] abilityCards;
    public List<AbilityCards> activeCards;
    private GameObject levelUI;
    public List<AbilityCards> chosenCards;
    private GameObject card1;
    private GameObject card2;
    private GameObject card3;
    private GameObject classCard;
    private Color cardColor;
    private GameObject enemyManager;
    private int selected = 0;
    private bool uiOn;


    void Start(){
        activeCards = new List<AbilityCards>();
        chosenCards = new List<AbilityCards>();
        enemyManager = GameObject.Find("EnemyManager");
        removable = true;
        initializeCards();
        uiOn = false;
        levelUI = GameObject.Find("LevelUpScreen");
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
                if(chosenCards[selected].name != "health"){
                    bool newAbility = false;
                    if(!activeCards.Contains(chosenCards[selected])){
                        newAbility = true;
                        AddAbility(chosenCards[selected]);
                        chosenCards[selected].UpTier();
                    } else{
                        chosenCards[selected].UpTier();
                        if(chosenCards[selected].tier >= 5){
                            RemoveCard(chosenCards[selected]);
                            }
                        }
                        if(selected == 0){ 
                            var cardScript = card1.GetComponent<AbilityCardDisaply>();
                            if(newAbility){
                                cardScript.ApplyCard();

                            }else{
                                cardScript.UpgradeCard();
                            }

                        }
                        else if (selected == 1){
                            var cardScript = card2.GetComponent<AbilityCardDisaply>();
                            if(newAbility){
                                cardScript.ApplyCard();
                            }else{
                                cardScript.UpgradeCard();
                            }


                        }
                    else if (selected == 2){
                        var cardScript = card3.GetComponent<AbilityCardDisaply>();
                        if(newAbility){
                            cardScript.ApplyCard();
                        }else{
                            cardScript.UpgradeCard();
                        }

                    }
                }
                //Debug.Log("Selected: "+selected);
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
//        Debug.Log("Display is being generated");
        chosenCards.Clear();
        if(abilityCards.Length > 3){
            while(chosenCards.Count < 3){
            int randomIndex = Random.Range(0,abilityCards.Length);
            if(!chosenCards.Contains(abilityCards[randomIndex])){
                //chosenCards.Add(abilityCards[randomIndex]);
                // if(activeCards.Contains(abilityCards[randomIndex])){
                //     abilityCards[randomIndex].UpTier();
                   
                // }
                if(abilityCards[randomIndex].tier < 5){
                    chosenCards.Add(abilityCards[randomIndex]);
                }
            }
            }
        } else{
            chosenCards = new List<AbilityCards>(abilityCards);
        }
        card1.GetComponent<AbilityCardDisaply>().card = chosenCards[0];
        card1.GetComponent<AbilityCardDisaply>().UpdateCard();
        card2.GetComponent<AbilityCardDisaply>().card = chosenCards[1];
        card2.GetComponent<AbilityCardDisaply>().UpdateCard();
        card3.GetComponent<AbilityCardDisaply>().card = chosenCards[2];
        card3.GetComponent<AbilityCardDisaply>().UpdateCard();
        //Print();
        uiOn = true;
        levelUI.SetActive(true);
        Time.timeScale = 0; //(This line pauses the game)
    }
    private void Print(){
        string output = "Cards generated: ";
        foreach(AbilityCards card in chosenCards ){
            output = output + card.name + " ";
        }
        //Debug.Log(output);
    }
    private void AddAbility(AbilityCards ability){
       // Debug.Log("adding ability: " +ability.name);
        activeCards.Add(ability);
    }
    void initializeCards(){
        foreach(AbilityCards card in abilityCards){
            card.initCard();
        }
    }
    void RemoveCard(AbilityCards card){
        if(removable){
            List<AbilityCards> temp = new List<AbilityCards>(abilityCards);
            temp.Remove(card);
            if(temp.Count < 3){
                while(temp.Count < 3){
                    //add health cards until there are 3 cards in the choosable pool
                    temp.Add(healthCard);
                }
                //removable = false;
            }
            abilityCards = temp.ToArray();  
        }
        
    }
    public AbilityCards ApplyClassCard(string name){
        //Debug.Log("Applying ability");
        string temp = "";
        int counter = 0;
        AbilityCards tempCard = null;
        while(temp != name){
            tempCard = abilityCards[counter];
            temp = tempCard.name;
            counter++;
        }
        //Debug.Log("TempCard: " + tempCard.name);
        tempCard.tier += 1;
        AddAbility(tempCard);
        tempCard.UpdateCard();
        return tempCard;
    }
    
}
