using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    // Public Vars
    public float movementSpeed, thrust;
    public Transform pivotPoint;
    public Rigidbody playerRb;

    // Private Vars
    private float startTime, elapsedTime;
    private int doublePressCounter;
    private KeyCode currentKey, lastKey;
    private bool dashMode, initDash = true;

    // Update is called once per frame
    private void Update(){
        playerMovement(playerInput());
        elapsedTime = Time.time - startTime;
        keyDownDetector();
    }

    private void keyDownDetector(){
        // Activate On Key Down
        if(Input.GetKeyDown(KeyCode.A)){ 
            lastKey    = currentKey == KeyCode.None ? KeyCode.A : currentKey;
            currentKey = KeyCode.A; 
            doublePressDetecter();
        } else if(Input.GetKeyDown(KeyCode.D)){
            lastKey    = currentKey == KeyCode.None ? KeyCode.D : currentKey;
            currentKey = KeyCode.D; 
            doublePressDetecter();
        } else if(Input.GetKeyDown(KeyCode.W)){
            lastKey    = currentKey == KeyCode.None ? KeyCode.W : currentKey;
            currentKey = KeyCode.W; 
            doublePressDetecter();
        } else if(Input.GetKeyDown(KeyCode.S)){
            lastKey    = currentKey == KeyCode.None ? KeyCode.S : currentKey;
            currentKey = KeyCode.S; 
            
            doublePressDetecter();
        }
    }

    private void doublePressDetecter(){
        if(currentKey != lastKey){
            return;
        }

        startTime = doublePressCounter == 0 ? Time.time : startTime;
        doublePressCounter = elapsedTime <= 2f ? doublePressCounter + 1 : 0;
        
        if(doublePressCounter == 2){
            currentKey         = KeyCode.None;
            lastKey            = KeyCode.None;
            dashMode           = true;
            initDash           = true;
            doublePressCounter = 0;
        } 
    }

    private void dashPlayerMovement(){
        playerRb.AddForce(transform.up * thrust);
    }

    private Vector3 playerInput(){
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        return movementInput;
    }

    private void playerMovement(Vector3 movement){
        if(movement == Vector3.zero){
            return;
        } 

        if(movement.normalized.x > 0){
            pivotPoint.eulerAngles = new Vector3(0f, 180f, 0f);
        } else if(movement.normalized.x < 0){
            pivotPoint.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        
        transform.Translate(movement * movementSpeed * Time.fixedDeltaTime);
    }
}
