using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Levels
{
  public int level;
  public int requiredXp;
}
[System.Serializable]
public class LevelsWrapper{
    public Levels[] levels;
}
