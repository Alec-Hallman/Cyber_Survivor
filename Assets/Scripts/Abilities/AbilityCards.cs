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
    public float value;
    public TextAsset json;
    public Abilities[] jsonArray;

    public void initCard(){
        tier = 0;
        name = jsonArray[tier].name;
        if(name == "Radius" || name == "Armor"){
            value = jsonArray[tier].value;
        }
        description = jsonArray[tier].description;
        AbilitiesWrapper abilitiesWrapper = JsonUtility.FromJson<AbilitiesWrapper>(json.text);
        jsonArray = abilitiesWrapper.abilities;
        //tier = -1;
        //Debug.Log(jsonArray.Length);
    }
    public void UpTier(){
        tier += 1;
        if(tier >= 5){
            tier = 5;
        }
        //name = jsonArray[tier].name;
        description = jsonArray[tier].description;
         if(name == "Radius" || name == "Armor"){
            value = jsonArray[tier - 1].value;
            //Debug.Log("AbilityCards has Radius: " +radius);
        }
        
        //Debug.Log("Description: " + description + " Tier: "+tier);
        //Debug.Log(jsonArray[tier].description);

    }

   
}
