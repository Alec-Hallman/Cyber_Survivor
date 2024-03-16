using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAbilitys : MonoBehaviour
{
    // Start is called before the first frame update
    private CircleCollider2D radiusTrigger;
    private PlayerBase playerScript;
    public GameObject[] avalibleWeapons;
    public GameObject hack;
    public GameObject missileFinder;
    private bool hackCheck;
    public WeaponBase weaponScript;
    public GameObject npcManager;
    private bool stealActive;
    private float time;
    private NPCManager npcScript;
    private float frequency;
    private Missle missleFinderScript;
    public List<GameObject>activeWeapons;
    private List<WeaponBase>weaponScripts;
    private SpawnScript enemyScript;
    private bool poisonActive;
    private AbilityCards poison;
    private AbilityCards lifeSteal;
    [SerializeField] GameObject puddleManager;
    void Start()
    {
        poisonActive = false;
        stealActive = false;
        weaponScripts = new List<WeaponBase>();
        activeWeapons = new List<GameObject>();
        enemyScript = GameObject.Find("EnemyManager").GetComponent<SpawnScript>(); 
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
        //Debug.Log("Card Name: " +card.name);
        //Debug.Log("Card Ability Name: " +card.abilityName);
        if(enemyScript == null){
            enemyScript = GameObject.Find("EnemyManager").GetComponent<SpawnScript>(); 
        }
        enemyScript.IncreaseDifficulty(card.difficulty);
//        Debug.Log("I was given a Card! Name: " +card.abilityName + "Tier: " +card.tier);
        if(card.abilityName == "Radius"){
//            Debug.Log("I was given Radius! It currently has radius: " + card.value);
            radiusTrigger = this.GetComponent<CircleCollider2D>();
            radiusTrigger.radius = card.value;
        } else if(card.abilityName == "Armor"){
            //Debug.Log("I was given Armor! Value is: " + card.value);
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

        }
        else if(card.abilityName == "LifeSteal"){
            stealActive = true;
            lifeSteal = card;
            ApplySteal(null);
        } else if(card.abilityName == "Poison"){
            poison = card;
            ApplyPoison(null);
        } else if(card.abilityName == "Missile"){
            //Debug.Log("I was given missile");
            GameObject finder = Instantiate(missileFinder);
            missleFinderScript = finder.GetComponent<Missle>();
            missleFinderScript.spawnTimer = card.value2;
            missleFinderScript.missleNumber = card.value;
        }else if(card.abilityName == "Shield"){
            if(playerScript == null){
                playerScript = GetComponent<PlayerBase>();
            }
            playerScript.projectileDodgeFactor = card.value;
        } else if(card.abilityName == "Phase"){
            if(playerScript == null){
                playerScript = GetComponent<PlayerBase>();
            }
            playerScript.phaseDurration = card.value;
            playerScript.passThrough = card.radioactive;
            playerScript.PhaseCooldown = card.value2;
        }else if (card.abilityName == "Brain-Cycle"){
            Debug.Log("Applied npcManager and other things.");
            GameObject tempNPC = Instantiate(npcManager,gameObject.transform); //Instantiate the npc manager as a child of the player.
            npcScript = tempNPC.GetComponent<NPCManager>();
            enemyScript.tranced = true; //Make it so enemies can create a npc when they die by the puddle.
            enemyScript.npcChance = card.value2; //Set the spawn chance to be the cards second value;
            enemyScript.NPChealth = card.value;


        } 
        else if(card.type == "Weapon"){
            int counter = 0;
            GameObject tempObj = avalibleWeapons[counter];
            while(tempObj.name != card.abilityName){
                tempObj = avalibleWeapons[counter];
                counter++;
            }
            GameObject weapon = Instantiate(tempObj, this.transform);
            activeWeapons.Add(weapon);
            //activeWeapons.Add(weapon);
            if(card.abilityName == "Katana"){
                WeaponBase tempScript;
                tempScript = weapon.GetComponent<WeaponBase>();
                tempScript.attackSpeed = card.value2;
                tempScript.damage = card.value;
                Vector3 scale = new Vector3(weapon.transform.localScale.x,(weapon.transform.localScale.y),0f);
                //weapon.transform.parent = this.transform;
                weapon.transform.localScale = (scale + scale);
                weapon.transform.position = new Vector3 ((weapon.transform.position.x + 100f), weapon.transform.position.y, 0f);
            }
            else if(card.name == "Pistol"){
                WeaponBase tempScript;
                tempScript = weapon.GetComponent<WeaponBase>();
                weapon.transform.localScale = new Vector2(2,2);
                tempScript.attackSpeed = card.value;
                tempScript.magazineSize = card.value2;
            } else if(card.name == "Puddle"){
                PuddleScript tempScript;
                tempScript = weapon.GetComponent<PuddleScript>();
                tempScript.size = card.value;
                tempScript.damage = card.value2;
            }
            if(poisonActive){
                ApplyPoison(weapon); //This only happens if poison has been chosen and a new weapon has just been made
            }
            if(stealActive){
                ApplySteal(weapon); //This only happens if lifesteal has been chosen and a new weapon has just been made
            }
            weaponScripts.Add(weapon.GetComponent<WeaponBase>());
        }
        
    }
    public void UpgradeAbility(AbilityCards card){
        enemyScript.IncreaseDifficulty(card.difficulty);
        Debug.Log("Card Name: " +card.name);
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
            lifeSteal = card;
            ApplySteal(null);
        }else if(card.abilityName == "Poison"){
            poison = card;
            ApplyPoison(null);
        } else if(card.abilityName == "Missile"){
            missleFinderScript.spawnTimer = card.value2;
            missleFinderScript.missleNumber = card.value;
        } else if(card.abilityName == "Shield"){
            playerScript.projectileDodgeFactor = card.value;
        } else if(card.abilityName == "Phase"){
            playerScript.phaseDurration = card.value;
            playerScript.passThrough = card.radioactive;
            playerScript.PhaseCooldown = card.value2;
        } else if(card.type == "Weapon"){
            //Debug.Log(weaponScripts[0].name + " Card Name: " + card.name);
            int counter = 0;
            Debug.Log("Upgradding a weapon");
            WeaponBase tempScript = weaponScripts[0];
            while(!tempScript.name.Contains(card.name)){
                counter++;
                tempScript = weaponScripts[counter];
            }
            //Everything above this point is for finding the actual weapon script to edit
            if(card.name == "Katana"){
                tempScript.attackSpeed = card.value2;
                tempScript.damage = card.value;
                tempScript.deflect = card.radioactive;
            } else if(card.name == "Pistol"){
                tempScript.attackSpeed = card.value;
                tempScript.magazineSize = card.value2;
                tempScript.noReload = card.radioactive;
            }else if(card.name == "Puddle"){
                Debug.Log("Upgradding a puddle weapon");
                tempScript.size = card.value;
                tempScript.damage = card.value2;
                tempScript.slow = card.radioactive;
            }
            
        }
    }
    private void GetTime(){
        time = Time.realtimeSinceStartup;
    }
    
    //For apply poison and steal, it being passes null just means: apply to every weapon.
        private void ApplyPoison(GameObject weapon){
        AbilityCards cardToUse;
        cardToUse = poison;
        if(weapon == null){ //if no weapon is given then apply to all weapons all upgrades for weapons should NOT provide a weapon
            foreach(GameObject weaponLoop in activeWeapons){
                WeaponBase script = weaponLoop.GetComponent<WeaponBase>();
                script.poison = true;
                script.pDamage = cardToUse.value;
                script.pDurration = cardToUse.value2;
                script.radioactive = cardToUse.radioactive;
                poisonActive = true;
            }
        }
        else{ //if a weapon object is provided only make these changes to the provided object (apply) 
            WeaponBase script = weapon.GetComponent<WeaponBase>();
            script.poison = true;
            script.pDamage = cardToUse.value;
            script.pDurration = cardToUse.value2;
            script.radioactive = cardToUse.radioactive;
            poisonActive = true;
        }
        
    }
    private void ApplySteal(GameObject weapon){
        AbilityCards cardToUse;
        cardToUse = lifeSteal;
        if(weapon == null){
            foreach(GameObject weaponLoop in activeWeapons){
            //Debug.Log("Setting steal to: " + card.value);
                weaponLoop.GetComponent<WeaponBase>().steal = cardToUse.value;
            }
        } else{
            weapon.GetComponent<WeaponBase>().steal = cardToUse.value;
        }
        
    }
}
