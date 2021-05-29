using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeGesture : MonoBehaviour
{
    // Public Vars
    public Player playerReference;
    public GameObject lineRenderPrefab;
    public enum Direction {None, Left, Up, Right, Down};

    // Private Vars
    private bool fadeOut;
    private float fadeOutSpeed = .1f;
    private Vector2 startPos, lastPos, swipe;
    private Vector3 linePos;
    private Direction direction;
    private LineRenderer oldLine, currentLine;

    private void Update(){
        checkSwipeInput();
    }
    
    private void FixedUpdate(){
        if(fadeOut){
            fadeOutSpeed += Time.deltaTime;
            // Need both to lerp to ensure the entire lines fades out evenly
            oldLine.endColor   = Color.Lerp(Color.green, new Color(0,0,0,0), fadeOutSpeed);
            oldLine.startColor = Color.Lerp(Color.green, new Color(0,0,0,0), fadeOutSpeed);
            if(oldLine.endColor == new Color(0,0,0,0) || oldLine.startColor == new Color(0,0,0,0)){
                Destroy(oldLine.gameObject);
                fadeOut = false;
            } 
        }
    }

    /*
        Function: Check Swipe Input, Params: None
        This function gets the initial start pos of current touch input.
        It draws a line according to where the current touch is being dragged, and 
        saves the last pos. Once the touch is released the direction of the swipe
        is checked. 
        
    */
    private void checkSwipeInput(){
        // L-Mouse clicked or screen touched
        if(Input.GetMouseButtonDown(0)){
            startPos = Input.mousePosition;
            currentLine = Instantiate(lineRenderPrefab).GetComponent<LineRenderer>();
        
        // L-Mouse held down or touch held down
        } else if(Input.GetMouseButton(0)){
            lastPos = Input.mousePosition;
            drawLine();
        
        // L-Mouse released or touch released
        } else if(Input.GetMouseButtonUp(0)){
            checkDirection();  
        }
    }
    
    /*
        Function: Check Direction, Params: None
        This function checks the direction of the swipe, by using the
        last position of the touch and the starting position. The direction
        is given to the player reference, and the other variables are reset.
    */
    private void checkDirection(){
        float swipeLength = 100f;
        swipe             = lastPos - startPos;

        if(swipe.magnitude > swipeLength){
            // Left or Right
            if(Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y)){
                direction = swipe.x > 0f ? Direction.Right : Direction.Left;
            }
            // Up or Down
            else{
                direction = swipe.y > 0f ? Direction.Up : Direction.Down;
            }
        }

        playerReference.GetComponent<Player>().sendDirection(direction);

        fadeOutLine();
        direction = Direction.None;
        lastPos   = startPos = Vector2.zero;
    }
    
    /*
        Function: Draw Line, Params: None
        This function draws a line between last end point, and 
        the new position.
    */
    private void drawLine(){
        // Set z to 10, otherwise line will not be drawn correctly
        linePos   = lastPos;
        linePos.z = 10f;
        linePos   = Camera.main.ScreenToWorldPoint(linePos);
        
        // Increase num of vectors and set end point to last pos.
        currentLine.positionCount++;            
        currentLine.SetPosition(currentLine.positionCount - 1, linePos);
    }

    /*
        Function: Fade Out Line, Params: None
        This function begins to slowly fade out the previously drawn line.
        If the previous line is not finished fading out before the current 
        line is done being drawn, then the previous line is destroyed.
    */
    private void fadeOutLine(){
        if(oldLine != null){
            Destroy(oldLine.gameObject);
        }
        oldLine = currentLine;
        fadeOutSpeed = .1f;
        fadeOut = true;
    }
}
