using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleObject : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform searcher;
    private Transform enemy;
    public float rotationRatio;
    public float maxSpeed;
    public float speed = 1f;
    public GameObject enemyObject;
    public string targetName;
    public float damage;
    private float flyTime = 1f;
    private Vector3 emergencyPosition;
    private ParticleSystem particles;
    private float startTime;
    private bool flying = false;
    private bool expand;
    private CircleCollider2D radius;
    private float maxRadius = 2.5f;
    void Start()
    {
        radius = gameObject.GetComponent<CircleCollider2D>();
        radius.radius = 1;
        startTime = Time.realtimeSinceStartup;
        this.transform.position = GameObject.Find("Player").GetComponent<Transform>().position;
        particles = gameObject.GetComponent<ParticleSystem>();
        searcher = GameObject.Find("MissleFinder").GetComponent<Transform>();
        enemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(expand && radius.radius < maxRadius){
            radius.radius += 0.5f;
        }
        if(speed < maxSpeed){
            speed *= 1.08f;
        }
        if(!particles.isPlaying){
            if(enemyObject != null && (Time.realtimeSinceStartup - startTime) > flyTime){
                flying = true;
                transform.position = Vector2.Lerp(transform.position, enemyObject.transform.position, Time.deltaTime * speed);
            }
            else if(searcher == null){
                transform.position = Vector2.Lerp(transform.position, emergencyPosition, Time.deltaTime * speed);
                    Invoke("SelfDestruct",1f);
            } else{
                transform.position = Vector2.Lerp(transform.position, searcher.position, Time.deltaTime * speed);
                if(enemyObject == null){
                    this.SetRandom();
                }
                //Quaternion rotation = Quaternion.LookRotation(searcher.position - transform.position, transform.TransformDirection(Vector3.up));
                //transform.rotation = new Quaternion (0,0,rotation.z + rotationRatio,rotation.w);
            }
        }
    }
    public void SetRandom(){
        searcher = null;
        emergencyPosition = (Vector2)this.transform.position + Random.insideUnitCircle * 5;
        speed = 1;
    }

    void OnTriggerEnter2D(Collider2D hit){
        if(hit.gameObject.name == targetName && flying){
            hit.gameObject.GetComponent<EnemyBase>().takeDamage(damage,false);
            SelfDestruct();
        } else if(expand && hit.gameObject.tag == "Enemy" && hit.gameObject.name != targetName){
            float distance = Vector3.Distance(this.transform.position, hit.gameObject.transform.position);
            distance *= 10;
            //If the distance multiplier makes it so that the missle will do negative damage just dont do the damage.
            if(distance < damage){
                hit.gameObject.GetComponent<EnemyBase>().takeDamage(damage - distance,false);
            }
        }
    }
    public void SelfDestruct(){
        expand = true;
        Invoke("BlowUp", 0.5f);
        if(!particles.isPlaying){
            particles.Play();
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    void BlowUp(){
        Destroy(this.gameObject);
    }
}
