using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class AbilityCardDisaply : MonoBehaviour
{
    // Start is called before the first frame update
    public AbilityCards card;
    public TMP_Text nameText;
    public TMP_Text description;
    public TMP_Text type;
    public Image icon; 
    public int tier;
    private GameObject player;
    private PlayerAbilitys playerScript;


    private void Start(){
        nameText.text = card.abilityName;
        description.text = card.description;
        type.text = card.type;
        icon.sprite = card.icon;

        player = GameObject.Find("Player");
//        Debug.Log(player.name);
        playerScript = player.GetComponent<PlayerAbilitys>();
    }

    public void UpdateCard()
    {
        
        nameText.text = card.abilityName;
        description.text = card.description;
        type.text = card.type;
        icon.sprite = card.icon;
        tier = card.tier;
//        Debug.Log("UpdateCard " +card.abilityName);
       // Debug.Log(playerScript);
        

       
    }
    public void ApplyCard(){
        playerScript.ApplyAbility(card);
    }
    public void UpgradeCard(){
        playerScript.UpgradeAbility(card);
    }
    
    // private void Update(){
    //     nameText.text = card.abilityName;
    //     description.text = card.description;
    //     type.text = card.type;
    //     icon.sprite = card.icon;

    // }
    
}
