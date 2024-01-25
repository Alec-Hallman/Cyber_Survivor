using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Class Card", menuName = "Class Card")]
public class ClassCards : ScriptableObject
{
    public string className;
    public string description;
    public Sprite abilityIcon;
    public Sprite weaponIcon;
    public int difficulty;
    public string ability;
    public string weapon;
    public TextAsset json;
    public void initCard(){
        Classes currentClass= JsonUtility.FromJson<Classes>(json.text);
        className = currentClass.name;
        description = currentClass.description;
        weapon = currentClass.weapon;
        ability = currentClass.ability;
    }
}
