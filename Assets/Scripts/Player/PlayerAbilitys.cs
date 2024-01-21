using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAbilitys : MonoBehaviour
{
    // Start is called before the first frame update
    private CircleCollider2D radiusTrigger;
    private PlayerBase playerScript;
    public GameObject hack;
    private bool hackCheck;
    public WeaponBase weaponScript;
    private float time;
    private float frequency;
    private GameObject[] weapons;
    void Start()
    {
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
       hackCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hackCheck && (Time.realtimeSinceStartup - time) > frequency){
            Instantiate(hack, transform);
            GetTime();
        }
    }
    public void ApplyAbility(AbilityCards card){
//        Debug.Log("I was given a Card! Name: " +card.abilityName + "Tier: " +card.tier);
        if(card.abilityName == "Radius"){
//            Debug.Log("I was given Radius! It currently has radius: " + card.value);
            radiusTrigger = this.GetComponent<CircleCollider2D>();
            radiusTrigger.radius = card.value;
        } else if(card.abilityName == "Armor"){
            Debug.Log("I was given Armor! Value is: " + card.value);
            if(playerScript == null){
                playerScript = GetComponent<PlayerBase>();
            }
            playerScript.resist = card.value;

        }
        else if(card.abilityName == "Hack"){
          //  Debug.Log("Hack Applied");
            GetTime();
            frequency = card.value2;
            hackCheck = true;
            hack.GetComponent<HackScript>().radius = card.value;

        } else if(card.abilityName == "LifeSteal"){
            foreach(GameObject weapon in weapons){
                weapon.GetComponent<WeaponBase>().steal = card.value;
            }
        } else if(card.abilityName == "Poison"){
            foreach(GameObject weapon in weapons){
                weapon.GetComponent<WeaponBase>().poison = true;
                weapon.GetComponent<WeaponBase>().pDamage = card.value;
                weapon.GetComponent<WeaponBase>().pDurration = card.value2;
            }
        }
    }
    public void UpgradeAbility(AbilityCards card){
        if(card.abilityName == "Radius"){
            //Debug.Log("Upgrading Radius! it has radius: " + card.value);
            radiusTrigger.radius = card.value;
        } else if(card.abilityName == "Armor"){
            //Debug.Log("I was given Armor! Value is: " + card.value);
            playerScript.resist = card.value;
        } else if (card.abilityName == "Hack"){
            frequency = card.value2;
            hack.GetComponent<HackScript>().radius = card.value;
        } else if(card.abilityName == "LifeSteal"){
            foreach(GameObject weapon in weapons){
                weapon.GetComponent<WeaponBase>().steal = card.value;
            }
        }else if(card.abilityName == "Poison"){
            foreach(GameObject weapon in weapons){
                weapon.GetComponent<WeaponBase>().pDamage = card.value;
                weapon.GetComponent<WeaponBase>().pDurration = card.value2;
                weapon.GetComponent<WeaponBase>().radioactive = card.radioactive;

            }
        }
    }
    private void GetTime(){
        time = Time.realtimeSinceStartup;
    }
}
