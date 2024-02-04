using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 200 == 0){
            float currentFPS = Time.frameCount / Time.time;
            text.text = (1/Time.deltaTime).ToString();
        }
        
    }
}
