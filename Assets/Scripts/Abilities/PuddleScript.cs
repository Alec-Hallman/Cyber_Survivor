using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleScript : WeaponBase
{
    // Start is called before the first frame update
    public float spawnFrequency;
    private float timer;
    [SerializeField] private GameObject puddle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.realtimeSinceStartup - timer > spawnFrequency){
            GameObject createdPuddle = Instantiate(puddle);
            createdPuddle.transform.position = this.transform.position;
            PuddleWeapon puddleScript = createdPuddle.GetComponent<PuddleWeapon>();
            puddleScript.maxSize = size;
            puddleScript.damage = damage;
            puddleScript.maxSize = size;
            puddleScript.poison = poison;
            puddleScript.pDamage = pDamage;
            puddleScript.pDurration = pDurration;
            GetTime();
        }
    }
    void GetTime(){
        timer = Time.realtimeSinceStartup;
    }
}
