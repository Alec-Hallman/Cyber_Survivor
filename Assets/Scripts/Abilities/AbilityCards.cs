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

   
}
