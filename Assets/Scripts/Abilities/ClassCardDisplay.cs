using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ClassCardDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public ClassCards card;
    public TMP_Text nameText;
    public TMP_Text description;
    public TMP_Text ability;
    public TMP_Text weapon;
    public Image weaponIcon;
    public Image abilityIcon; 
    private GameObject manager;
    private AbilityManager managerScript;


    private void Start(){
        card.initCard();
        nameText.text = card.className;
        description.text = card.description;
        weapon.text = card.weapon;
        ability.text = card.ability;
        weaponIcon.sprite = card.weaponIcon;
        abilityIcon.sprite = card.abilityIcon;
        //manager = GameObject.Find("Manager");
//        Debug.Log(player.name);
        //managerScript = manager.GetComponent<AbilityManager>();
    }

    public void ApplyClassCard(){
       // playerScript.ApplyClass(card);
       //managerScript.ApplyClassCard(card.weapon);
       //managerScript.ApplyClassCard(card.ability);


    }
    public void UpgradeCard(){
       // playerScript.UpgradeAbility(card);
    }
    
    
    // private void Update(){
    //     nameText.text = card.abilityName;
    //     description.text = card.description;
    //     type.text = card.type;
    //     icon.sprite = card.icon;

    // }
    
}
