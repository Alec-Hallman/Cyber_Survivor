using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Abilities
{
    public string name;
    public string type;
    public int tier;
    public bool pair;
    public string description;
    public float value;
    public float value2;
}
[System.Serializable]
public class AbilitiesWrapper{
    public Abilities[] abilities;
}
