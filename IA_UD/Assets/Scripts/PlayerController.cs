using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    // Public Vars
    public float movementSpeed, jumpHeight, jumpThrust, dashSpeed, maxDashDistance;
    public Transform pivotPoint;
    public Rigidbody playerRb;

    // Private Vars
    private Vector3 dashStartPos, dashCurrentPos, lastPlayerMovementInput;
    private KeyCode currentKey, lastKey, dashDirection;
    private bool dashMode, jumpMode;

    // Update is called once per frame
    private void Update(){
        dashCurrentPos = transform.position; 
        
        //Player must perform all these before dashing
        if(!dashMode){
            playerMovement(playerInput());
            dashDetecter();
            jumpDetector();        
        }else{
            dashPlayerMovement();
        }
    }

    /* Checks if the rigidbody is grounded*/
    private void OnCollisionEnter(Collision other) {
        jumpMode = false;
    }

    /*
        Function: Jump Detector, Params: None
        This function is used to detect if the player is attempting to jump. 
        If the space bar is pressed and the player is not currently off the ground,
        the jump mode is enabled.
    */
    private void jumpDetector(){        
        if(Input.GetKeyDown(KeyCode.Space) && !jumpMode){
            jumpPlayerMovement();
            jumpMode  = true;
        } 
    }

    /*
        Function: Dash Detector, Params: None
        This function is used to detect if the player is attempting to dash.
        It checks if the l-shift key is being pressed, and if true dash mode is
        enabled.
    */
    private void dashDetecter(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            lastPlayerMovementInput = playerInput();
            dashStartPos            = dashCurrentPos;
            dashMode                = true;
        }
    }
    
    /*
        Function: Player Input, Params: None, Return: Vector3
        This function is used to detect player input from the A, W, S, D
        and arrow keys. A vector3 is assembled using the users input and
        is returned.
    */
    private Vector3 playerInput(){
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        return movementInput;
    }

    /*
        Function: Jump Player Movement, Params: None
        This function applies an upward force on the player rigidbody.
    */
    private void jumpPlayerMovement(){
        playerRb.AddForce(new Vector3(0f, jumpHeight, 0f) * jumpThrust, ForceMode.Impulse);
    }

    /*
        Function: Dash Player Movement, Params: None
        This function applies a dash like force on the player.
        If the player was not in motion when dash mode was enabled,
        dash mode is disabled. Dash mode will stay enabled until
        the dash distance threshold is exceeded. 
    */
    private void dashPlayerMovement(){
        if(lastPlayerMovementInput == Vector3.zero){
            dashMode = false;
            return;
        }

        if((dashCurrentPos - dashStartPos).magnitude < maxDashDistance){
            transform.Translate(lastPlayerMovementInput * dashSpeed * Time.fixedDeltaTime);
        } else{         
            dashMode = false;
        }
    }

    /*
        Function: Player Movement, Params: None
        This function applies movement to the player.
        If there is no input in the vector then there is no
        need to calculate movement. If the input identifies 
        a change in direction, a rotation of the sprite(Left and Right)
        is made.   
    */
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
