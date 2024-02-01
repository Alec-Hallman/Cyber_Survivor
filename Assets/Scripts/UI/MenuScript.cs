using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI[] textOptions;
    private int selection = 0;
    private int oldSelection;
    public GameObject classMenu;
    void Start()
    {
        textOptions = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){
            if(selection == 1){
                classMenu.SetActive(true);
                //TurnOff();
                //SceneManager.LoadScene("TheGame");
            }else{
                //TurnOff();
            }
        } else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)){
            oldSelection = selection;
            selection -= 1;
            if(selection < 1){
                selection = 3;
            }
        } else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)){
            oldSelection = selection;
            selection += 1;
            selection = selection % 4;
            if(selection == 0){
                selection += 1;
            }
        }
        if(oldSelection != selection){
            textOptions[oldSelection].GetComponent<TextMeshProUGUI>().color = Color.white;
            textOptions[selection].GetComponent<TextMeshProUGUI>().color = Color.blue;
        }   
    }
    private void TurnOff(){
        foreach(TextMeshProUGUI text in textOptions){
            text.enabled = false;
        }
    }
    public void TurnOn(){
        foreach(TextMeshProUGUI text in textOptions){
            text.enabled = false;

        }
    }
}
