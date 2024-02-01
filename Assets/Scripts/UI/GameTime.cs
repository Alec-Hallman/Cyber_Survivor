using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        int minutes = (int)Time.realtimeSinceStartup / 60;
        int seconds = (int)Time.realtimeSinceStartup % 60;
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
