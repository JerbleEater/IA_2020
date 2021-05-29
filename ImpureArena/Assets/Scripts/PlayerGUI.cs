using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour{   
    // Public Vars
    public GameObject heartPrefab, heartPanel, visualInventory_content, bottomCanvas;

    // Private Vars
    private bool inSwipeState, movingItem;
    private float previousHeartXPos = 0;
    private List<Animator> onScreenHeartList;

    private void Awake(){
        onScreenHeartList = new List<Animator>();
    }

    /*
        Function: Initial Heart Spawner, Params: Float
        This function calls the coroutine heartSpawner() for 
        each player hit point
    */
    public void initialHeartSpawner(int playerHp){
        for(int i = 0; i < playerHp; i++){
            StartCoroutine(heartSpawner(.25f * (i+2)));
        }
    }

    /*
        Function: Accessing Pause Menu, Params: None
        This function stops swiping gestures.
    */
    public void accessingPauseMenu(){
        inSwipeState = false;
    }

    /*
        Function: Accessing Swipe Gesture, Params: None
        This function turns swiping gestures on.
    */
    public void accessingSwipeGesture(){
        inSwipeState = true;
    }

    /*
        Function: Decrease Hearts On Screen, Params: Float
        This function removes hearts visually. It resets the
        previousHeartXPos offset, and certain animations are played
        depending on players current hp.
    */
    public void decreaseHeartsOnScreen(float playerHp){
        previousHeartXPos -= 65f;

        if(playerHp >= 0){
            onScreenHeartList[onScreenHeartList.Count - 1].Play("explodingHeart");
            onScreenHeartList.RemoveAt(onScreenHeartList.Count - 1);
        }

        if(playerHp == 1){
            onScreenHeartList[0].Play("shakingHeart");
        }
    }

    /*
        Function: Decrease Hearts On Screen, Params: None
        This function removes hearts visually. It reset the
        previousHeartXPos, and certain animations are played
        depending on players current hp.
    */
    public void increaseHeartsOnScreen(){
        onScreenHeartList[0].Play("idle");
        StartCoroutine(heartSpawner(0f));
    }

    /*
        Function: Heart Spawner, Params: Float
        This function instantiates a heart after the specifed delay.
        The heart are spawned at an offset of the previous hearts x pos, and
        the initial heart animations are played.
    */
    private IEnumerator heartSpawner(float delayInSeconds){
        yield return new WaitForSeconds(delayInSeconds); 
        GameObject go     = Instantiate(heartPrefab, heartPanel.transform);
        previousHeartXPos = previousHeartXPos == 0 ? go.GetComponent<RectTransform>().localPosition.x : previousHeartXPos + 65f; 
        go.GetComponent<RectTransform>().localPosition = new Vector3(previousHeartXPos, 
                                                                    go.GetComponent<RectTransform>().localPosition.y, 
                                                                    go.GetComponent<RectTransform>().localPosition.z);
        onScreenHeartList.Add(go.GetComponent<Animator>());
    }

    // Getters
    public bool getSwipeState(){
        return inSwipeState;
    }

}
