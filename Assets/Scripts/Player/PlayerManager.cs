using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject levelUI;
    public int level;
    private int neededXp;
    private int currentXp;
    private Levels[] levelInfo;
    private GameObject xpBar;
    private float xChange;
    private Vector3 scaleChange;
    private bool levelingUp;
    private int selected = 0;
    private GameObject card1;
    private GameObject card2;
    private GameObject card3;
    private Color cardColor;
    
    private List<AbilitiesWrapper> abilities;
    void Start()
    {

        card1 = GameObject.Find("Card1");
        card2 = GameObject.Find("Card2");
        card3 = GameObject.Find("Card3");
        cardColor = card1.GetComponent<Image>().color;

        levelingUp = false;
        levelUI = GameObject.Find("LevelUpScreen");  
        levelUI.SetActive(false); 
        xpBar = GameObject.Find("Xp");
        xpBar.transform.localScale = new Vector2(0f,0.2125f);
        //Debug.Log("Xp Bar is: " +xpBar.name);
        level = 0;
        string json = File.ReadAllText("Assets/Jsons/Levels.json");
        LevelsWrapper levelInfoArray = JsonUtility.FromJson<LevelsWrapper>("{\"levels\":" + json + "}");
        levelInfo = levelInfoArray.levels;
        neededXp = levelInfo[level].requiredXp;
        Debug.Log("Needed xp: " + neededXp);
        xChange = 11.627f / neededXp;
        scaleChange = new Vector3(xChange,0f,0f);
        //Debug.Log("Assets/Weapons/"+ classInfo.weapon +".prefab");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            IncreaseXp(1);
            Debug.Log("Xp Needed to level up: " + neededXp);
        }
        if(levelingUp){
            
            
            if(Input.GetKeyDown(KeyCode.Escape)){
                levelingUp = false;
                levelUI.SetActive(false);
                Time.timeScale = 1; 

            }
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                selected = (selected + 1) % 3;
                Debug.Log("Selected: #" + selected);
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                selected = selected - 1;
                if(selected == -1){
                    selected = 2;
                }
                Debug.Log("Selected: #" + selected);
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
    void IncreaseXp(int ammount){
        currentXp += ammount;
        xpBar.transform.localScale += scaleChange;
        if(currentXp >= neededXp){
            LevelUp();
        }
    }
    void LevelUp(){
        levelingUp = true;
        Time.timeScale = 0; //(This line pauses the game)
        levelUI.SetActive(true);
        level += 1;
       // loadAbility("Radius");
        currentXp = currentXp - neededXp;
        neededXp = levelInfo[level].requiredXp;
        xpBar.transform.localScale = new Vector2(0f,0.2125f);
        xChange = 11.627f / neededXp;
        scaleChange = new Vector3(xChange,0f,0f);
        if(currentXp != 0){
            for(int i = 0; i < currentXp; i++){
                xpBar.transform.localScale += scaleChange;
            }
        }

    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "XP"){
            int ammount = 0;
           if(col.name.Contains("Green")){
            ammount = 1;
           }
            if(col.name.Contains("Purple")){
            ammount = 2;
           }
            IncreaseXp(ammount);
            Destroy(col.gameObject);
        }
    }
    void UpdateAbilities(string name){
        
    }
    void loadAbility(string name){
        string json = File.ReadAllText("Assets/Jsons/Abilities/"+name+".json");
        AbilitiesWrapper levelInfoArray = JsonUtility.FromJson<AbilitiesWrapper>("{\"radius\":" + json + "}");
        abilities.Add(levelInfoArray);
        
    }
}
