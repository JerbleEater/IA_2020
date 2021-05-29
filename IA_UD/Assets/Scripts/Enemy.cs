using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Public Vars
    public EnemySpriteDatabase enemySprites;

    // Private Vars
    private SwipeGesture.Direction currDirection;
    private Animator animArrow;
    private Slider hpSlider;
    private string animToPlay;
    private float hp, movementSpeed;

    private void Awake(){
        // Assign hp and attach enemySpritePrefab to the canvas of the new enemy prefab
        hp            = enemySprites.randomEnemySelector(this.transform.GetChild(0));
        movementSpeed = 50f;
        animArrow     = gameObject.GetComponentInChildren<Animator>();
        hpSlider      = gameObject.GetComponentInChildren<Slider>();
        
        hpSlider.maxValue = hp;
        hpSlider.value    = hp;
    }

    private void Start(){
        generateDirection();
    }

    private void FixedUpdate() { 
        // Ensures animator is displaying correct arrow direction
        if(!animArrow.GetCurrentAnimatorStateInfo(0).IsName(animToPlay)){
            animArrow.Play(animToPlay);
        }
        // Moves enemy forward
        this.transform.position += -Vector3.forward * Time.deltaTime * movementSpeed;
    }

    /*
        Function: On Trigger Enter, Params: Collider
        This function checks if the box collider overlaps with the KillBox collider
        a call is made to the GameHandler and the enemy obj is destroyed. 
    */
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "KillBox"){
            GameHandler.Instance.fuckPlayerOneUp();
            Destroy(gameObject);
        }    
    }

    /*
        Function: Generate Direction, Params: None
        This function uses a switch statement to determine which arrow direction
        to assign the enemy, along with which direciton animation to begin playing.  
    */
    private void generateDirection(){

        int rand = Random.Range(0, 4);

        switch(rand){
            case 0:
                currDirection = SwipeGesture.Direction.Left;
                animToPlay = "left";
                break;
            case 1:
                currDirection = SwipeGesture.Direction.Up;
                animToPlay = "up";
                break;
            case 2:
                currDirection = SwipeGesture.Direction.Right;
                animToPlay = "right";
                break;
            case 3:
                currDirection = SwipeGesture.Direction.Down;
                animToPlay = "down";
                break;
        }
        animArrow.Play(animToPlay);
    }

    /*
        Function: Decrease Hp, Params: Float
        This function subtracts the amount of damage provided from
        the current hp. Visually changes the hp slider value as well.
        If current hp is less than 0, the enemy obj is destroyed.   
    */
    private void decreaseHp(float damage){
        hp -= damage;
        hpSlider.value = hp;
        
        if(hp <= 0.0f){
            Destroy(this.gameObject);
        }
    }

    /*
        Function: Check Current Direction, Params: SwipeGesture.Direction, Float
        This function checks if the player direction matches the enemies arrow direction.
        If matched, enemy hp is decreased based on players total damage. A new arrow direction
        is generated.  
    */
    public void checkCurrentDirection(SwipeGesture.Direction playerDirection, float damage){
        if(playerDirection == currDirection){
            decreaseHp(damage);  
            // Ensures repeat direction are played from the beginning.
            animArrow.Play("start");      
            generateDirection();
        }
    }

}
