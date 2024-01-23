using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Ability Card", menuName = "Ability Card")]
public class AbilityCards : ScriptableObject
{
    public string abilityName;
    public string type;
    public string description;
    public Sprite icon;
    public int tier;
    public int difficulty;
    public float value;
    public float value2;
    public bool radioactive;
    public TextAsset json;
    public Abilities[] jsonArray;

    public void initCard(){
        tier = 0;
        name = jsonArray[tier].name;
        value = jsonArray[tier].value;
        value2 = jsonArray[tier].value2;
        description = jsonArray[tier].description;
        radioactive = jsonArray[tier].radioactive;
        difficulty = jsonArray[tier].difficulty;
        AbilitiesWrapper abilitiesWrapper = JsonUtility.FromJson<AbilitiesWrapper>(json.text);
        jsonArray = abilitiesWrapper.abilities;
        //tier = -1;
        //Debug.Log(jsonArray.Length);
    }
    public void UpTier(){
        Debug.Log("Difficulty" + jsonArray[tier].difficulty);
        tier += 1;
        //string message = "";
        // for(int i = 0; i < 5; i++){
        //     message += " Diff: " + jsonArray[i].difficulty.ToString(); 
        // }
        //Debug.Log(message);

        //Debug.Log("Upgrading Tier for: " + jsonArray[tier].name + "Tier: " + tier);
        if(tier >= 5){
            tier = 5;
        }
        //name = jsonArray[tier].name;
        radioactive = jsonArray[tier - 1].radioactive;
        difficulty = jsonArray[tier - 1].difficulty;
        Debug.Log("Difficulty" + jsonArray[tier].difficulty);
        description = jsonArray[tier].description;
        value = jsonArray[tier - 1].value;
        value2 = jsonArray[tier - 1].value2;
            //Debug.Log("AbilityCards has Radius: " +radius);
        
        //Debug.Log("Description: " + description + " Tier: "+tier);
        //Debug.Log(jsonArray[tier].description);

    }

   
}
