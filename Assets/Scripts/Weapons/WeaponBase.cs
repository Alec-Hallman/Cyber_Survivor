using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//Player Weapon base behaviour
public class WeaponBase : MonoBehaviour
{
    public float damage;
    protected bool dealDamage;
    public float magazineSize;
    public float attackSpeed;
    public float attackInterval;
    public float projectileCount;
    public float radius;
    [SerializeField] protected float maxDistance;
    public GameObject player;
    public float pDamage;
    public float pDurration;
    public bool swinging;
    public bool poison;
    public bool radioactive;
    public bool deflect;
    public float steal = 0;
    public bool noReload;
    public float size;
    //protected HashSet<GameObject>enemiesInRange;
    //protected HashSet<GameObject>enemiesToRemove;
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
    public float GetDistance(GameObject objectToCheck){
        float distance = Vector2.Distance(this.transform.position, objectToCheck.transform.position);
        return distance;
    }
}