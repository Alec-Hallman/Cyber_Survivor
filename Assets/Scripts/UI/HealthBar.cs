using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private PlayerBase playerScript;
    private float maxHealth;
    private float maxXScale;
    private float movePerHealth;
    private float currentScale;
    private float yScale;
    private Animator barAnimation;
    // Start is called before the first frame update
    void Start()
    {
        barAnimation = GetComponentInParent<Animator>();
        yScale = this.transform.localScale.y;
        playerScript = GameObject.Find("Player").GetComponent<PlayerBase>();
        maxXScale = this.transform.localScale.x;
        currentScale = maxXScale;
        maxHealth = playerScript.maxHealth; 
        movePerHealth = maxXScale / maxHealth;
        //Debug.Log("Health Bump = " + movePerHealth / 100 + "Lets Check that math: " + movePerHealth);
        this.transform.localScale = new Vector2(currentScale,yScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReduceHealthBar(float damage){
        barAnimation.SetBool("Done", false);
        barAnimation.SetBool("Appear", true);
        Invoke("FadeOut", 1f);
        if(currentScale - (movePerHealth * damage) > 0){
            currentScale = currentScale - (movePerHealth * damage);
        } else{
            currentScale = 0;
        }
        this.transform.localScale = new Vector2(currentScale,yScale);
    }
    public void IncreaseHealthBar(float ammount){
        barAnimation.SetBool("Done", false);
        barAnimation.SetBool("Appear", true);
        currentScale = currentScale + (movePerHealth * ammount);
        this.transform.localScale = new Vector2(currentScale,yScale);
    }
    private void FadeOut(){
        barAnimation.SetBool("Appear", false);
        Invoke("Done",0.5f);
    }
    private void Done(){
        barAnimation.SetBool("Done", true);
    }
}
