using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManaget : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] objects;
    public int spawnCount;
    void Start()
    {

        int length = objects.Length;
        for(int i = 0; i < spawnCount; i++){
            //Debug.Log("Running Loop");
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(objects[0], new Vector3(randomPosition.x, randomPosition.y, 0f), Quaternion.identity);

        }
    }
    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = Random.insideUnitSphere * 400f;
        randomPosition += transform.position;
        return randomPosition;
    }
}
