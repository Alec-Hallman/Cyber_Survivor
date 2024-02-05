using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float speed;
    private NPCManager npcScript;
    private Queue<GameObject> tracker;
    private Transform targetTransform;
    private Vector2 direction;
    public GameObject player;
    private float distance;
    private UIManager UIScript;
    private Rigidbody2D rigid;
    private EnemyBase enemyScript;
    public float damage;
    public float attackSpeed;
    private Vector3 ZERO;
    void Start()
    {
        ZERO = new Vector3(0,0,0);
        enemyScript = null;
        player = GameObject.Find("Player");
        UIScript = GameObject.Find("Canvas").GetComponent<UIManager>();
        tracker = new Queue<GameObject>();
        npcScript = GameObject.Find("NPCManager(Clone)").GetComponent<NPCManager>();
        targetTransform = npcScript.GetEnemy();
        rigid = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(player.transform.position, this.transform.position);
        if(distance > 20f){
            Destroy(this.gameObject);
        }
       if(targetTransform != null){
            direction = (targetTransform.position - transform.position).normalized;
            rigid.velocity = direction * speed;
       } else{
            rigid.velocity = ZERO;
            targetTransform = npcScript.GetEnemy();
            //Debug.Log("Getting New Enemy");
       }
    }
    public void TakeDamage(float damage){
        UIScript.DisplayHit(damage, this.gameObject, false ,false);
        health =- damage;
        if(health <= 0){
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D hit){
        if(hit.gameObject.tag.Contains("Enemy") && enemyScript == null){
            enemyScript = hit.gameObject.GetComponent<EnemyBase>();
            Invoke("DealDamage",attackSpeed);
        }
    }
    void DealDamage(){
        if(enemyScript != null){
            enemyScript.takeDamage(damage, false);
            //Debug.Log("Dealing Damage to enemy");
            Invoke("DealDamage", attackSpeed);
        }
    }
}
