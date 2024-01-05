using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class Abilities
{
    public string name;
    public string type;
    public int tier;
    public bool pair;
    public string description;

    public abstract void Activate(GameObject something);

}
public class Radius : Abilities
{
    int circleRadius;
    public override void Activate(GameObject player){
        player.GetComponent<CircleCollider2D>().radius = circleRadius;
    }
}
[System.Serializable]
public class AbilitiesWrapper{
    public Abilities[] abilities;
}
