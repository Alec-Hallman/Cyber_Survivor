using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClassSelectionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public ClassCardDisplay[] cards;
    public Image[] backgrounds; 
    private int selection = 0;
    private int oldSelection;

    // Update is called once per frame
    void Start(){
        this.gameObject.SetActive(false);
        cards = GetComponentsInChildren<ClassCardDisplay>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){
            PlayerPrefs.SetString("Class", cards[selection].card.name);
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("TheGame");

        } else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)){
            oldSelection = selection;
            selection -= 1;
            if(selection < 0){
                selection = cards.Length - 1;
            }
        } else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)){
            oldSelection = selection;
            selection += 1;
            selection = selection % cards.Length;
        }
        if(oldSelection != selection){
            backgrounds[oldSelection].color = Color.white;
            backgrounds[selection].color = Color.blue;
        }   
    }
}
