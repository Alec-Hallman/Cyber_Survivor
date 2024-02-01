using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Map theMap;
    void Start()
    {
        theMap = new Map();
        theMap.MapInit(100,100);
    }
}
