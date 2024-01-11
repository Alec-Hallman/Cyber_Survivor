using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int level;
    private float neededXp;
    private float currentXp;
    private Levels[] levelInfo;
    private GameObject xpBar;
    private float xChange;
    private Vector3 scaleChange;
    private GameObject manager;
    private float xpNeeded;
    
    private List<AbilitiesWrapper> abilities;
    void Start()
    {
        xpNeeded = 5f;
        manager = GameObject.Find("Manager");
        xpBar = GameObject.Find("Xp");
        xpBar.transform.localScale = new Vector2(0f,0.2125f);
        //Debug.Log("Xp Bar is: " +xpBar.name);
        level = 0;
        string json = File.ReadAllText("Assets/Jsons/Levels.json");
        LevelsWrapper levelInfoArray = JsonUtility.FromJson<LevelsWrapper>("{\"levels\":" + json + "}");
        levelInfo = levelInfoArray.levels;
        neededXp = levelInfo[level].requiredXp;
       // Debug.Log("Needed xp: " + neededXp);
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
            //Debug.Log("Xp Needed to level up: " + neededXp);
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
        manager.GetComponent<AbilityManager>().GenerateDisplay();
        level += 1;
        currentXp = currentXp - neededXp;
        xpNeeded *= 1.15f;
        neededXp = xpNeeded;
        xpBar.transform.localScale = new Vector2(0f,0.2125f);
        xChange = 11.627f / neededXp;
        scaleChange = new Vector3(xChange,0f,0f);
        if(currentXp != 0){
            for(int i = 0; i < currentXp; i++){
                xpBar.transform.localScale += scaleChange;
            }
        }

    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "XP"){
            int ammount = 0;
            if(col.gameObject.name.Contains("Green")){
            ammount = 1;
           }
            if(col.gameObject.name.Contains("Purple")){
            ammount = 5;
           }
            IncreaseXp(ammount);
            //Destroy(col.gameObject);
        }
    }
    void loadAbility(string name){
        string json = File.ReadAllText("Assets/Jsons/Abilities/"+name+".json");
        AbilitiesWrapper levelInfoArray = JsonUtility.FromJson<AbilitiesWrapper>("{\"radius\":" + json + "}");
        abilities.Add(levelInfoArray);
        
    }
}
