using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    // Private Vars
    private Image bg;
    private int counter;
    private Animator title;
    private float startTime, elapsedTime;

    private void Start()
    {
        bg    = this.GetComponentInChildren<Image>();
        title = this.GetComponentInChildren<Animator>();
    }

    private void FixedUpdate(){
        bg.transform.Rotate(0, 0, 10 * Time.deltaTime);
        elapsedTime = Time.time - startTime;
    }

    public void TitlePressed(){
        title.Play("Base Layer.ShockState", 0, 0.25f);
        // Begin timer on first click
        startTime = counter == 0f ? Time.time : startTime;
        // Increase counter if click is within the elapsed time
        counter = elapsedTime <= 5f ? counter + 1 : 0;
        
        // Enter EE if title clicked 10 times
        if(counter == 10f){
            counter = 0;
            Debug.Log("Enter the ee");
        } else{
            Debug.Log("ElapsedTime: " + (Time.time - startTime).ToString());
            Debug.Log("Counter: " + counter.ToString());
        }
    }

    public void PlayPressed(){
        SceneManager.LoadScene("Level");
    }

    public void UpgradePressed(){

    }

    public void OptionsPressed(){

    }

    public void QuitPressed(){
        
    }

}
