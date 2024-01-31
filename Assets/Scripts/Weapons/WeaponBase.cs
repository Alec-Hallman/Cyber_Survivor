using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//Player Weapon base behaviour
public class WeaponBase : MonoBehaviour
{
    public float damage;
    public float magazineSize;
    public float attackSpeed;
    public float attackInterval;
    public float radius;
    public GameObject player;
    public float pDamage;
    public float pDurration;
    public bool swinging;
    public bool poison;
    public bool radioactive;
    public bool deflect;
    public float steal = 0;
    // Start is called before the first frame update
    public void WeaponStart()
    {
        player = GameObject.Find("Player");
        GetComponent<CircleCollider2D>().radius = radius;
    }
    public void LifeSteal(){
//        Debug.Log("Calling Steal Steal value" + steal);
        player.GetComponent<PlayerBase>().GainHealth(damage * steal);
    }
}