using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Private Vars
    private float globalLvl, globalDamage, globalCritChance, xpAdded, damageAdded, critChanceAdded, critMultiplier = 2.5f;
    private int inGameHp, hpAdded, globalHp = 4;
    private Item selectedItem;
    private PlayerGUI playerGUI;
    private PlayerInventory playerInventory;
    
    private void Awake(){
        playerGUI       = GameObject.Find("PlayerGUI").GetComponent<PlayerGUI>();
        playerInventory = gameObject.AddComponent<PlayerInventory>();
        inGameHp        = globalHp;
    }

    private void Start(){
        playerGUI.initialHeartSpawner(inGameHp); 
    }

    /*
        Function: Total Damage, Params: None, Return: Float
        This function determine the total amount of damage that player is capable of 
        taking away from enemy hp. Total damage is calculated using crit chance, item 
        crit chance, player damage, and item damage. Total damage is returned
    */
    private float totalDamage(){ 
        float totalCritChance = globalCritChance + selectedItem.getItemCritChanceAdded(); 
        float totalDamage     = globalDamage + selectedItem.getItemDamageAdded();     
        totalDamage           = Random.Range(0, 101) < totalCritChance ? totalDamage * critMultiplier : totalDamage;
        return totalDamage;
    }

    /*
        Function: Increase Level, Params: None
        This function increases the players true level.
    */
    public void increaseLevel(){
        globalLvl += xpAdded;
    }

    /*
        Function: Increase Global Hp, Params: None
        This function increases the players true health.
    */
    public void increaseGlobalHp(){
        globalHp += hpAdded; 
    }

    /*
        Function: Increase in Game Hp, Params: None
        This function increases the players health during a given game.
        (Refer to increaseGlobalHp() for players true health)
    */
    public void increaseInGameHp(){
        inGameHp += 1;
        playerGUI.increaseHeartsOnScreen();
    }

    /*
        Function: Decrease in Game Hp, Params: None
        This function decreases the players health during a given game.
        Visual hearts are removed from the playerGUI. If in game health
        is below 0, the game is ended.
    */
    public void decreaseInGameHp(){
        inGameHp -= 1;
        playerGUI.decreaseHeartsOnScreen(inGameHp);

        if(inGameHp <= 0){
            Debug.Log("Your dead fuck face");
        } 
    }

    /*
        Function: Increase Damage, Params: None
        This function increase the players true damage. 
    */
    public void increaseDamage(){
        globalDamage += damageAdded;
    }

    /*
        Function: Increase Crit, Params: None
        This function increase the players true crit chance. 
    */
    public void increaseCritChance(){
        globalCritChance += critChanceAdded;
    }

    /*
        Function: Send Direction, Params: SwipeGesture.Direction
        This function sends the players direction and players global damage to the 
        game handler to be compared against all spawned enemies arrow directions.
    */
    public void sendDirection(SwipeGesture.Direction swipeDirection){
        // Second Param should be totalDamage()...Currently Testing
        GameHandler.Instance.fuckEnemyUp(swipeDirection, 10f);
    }

    /*
        Function: Add item to inventory, Params: GameObject, Item.ItemType
        This function checks if the itemType provided exist in the playerInventory.
        If the itemType exist, the item count in the playerInventory is incremented by 1
        and the grabbed item is destroyed. Otherwise, the item is added to the playerInventory
        and is also added visually.
    */
    public void addItemToInventory(GameObject itemGrabbed, Item.ItemType itemType){
        if(playerInventory.getItemDict().ContainsKey(itemType)){
            playerInventory.getItemDict()[itemType] += 1;
            Destroy(itemGrabbed);
        } else{
            playerInventory.getItemDict().Add(itemType, 1);
            itemGrabbed.transform.SetParent(playerGUI.visualInventory_content.transform);
            itemGrabbed.transform.localScale = new Vector3(.7f, .7f, .7f);
            itemGrabbed.GetComponent<RectTransform>().sizeDelta = new Vector2(35f, 35f);
        }
    }

    /*
        Function: Remove item from inventory, Params: GameObject, Item.ItemType
        This function assumes the item exist in the playerInventory(Might not be a safe assumption), 
        and decrements the item count. If the itemType is the last item in the playerInventory it 
        is removed from the playerInventory, otherwise the go is instantiated to replace the object
        visually in the playerInventory. 
    */
    public void removeItemFromInventory(GameObject itemGrabbed, Item.ItemType itemType){ 
        playerInventory.getItemDict()[itemType] -= 1;
        if(playerInventory.getItemDict()[itemType] <= 0){
            playerInventory.getItemDict().Remove(itemType);   
        } else{
            GameObject go = Instantiate(itemGrabbed, playerGUI.visualInventory_content.transform);
            go.GetComponent<Item>().setItemState(Item.CurrentState.InInventory);
        }
        itemGrabbed.transform.SetParent(playerGUI.bottomCanvas.transform);
    }

    // Getters
    public int getInGameHp(){
        return inGameHp;
    }

    public PlayerGUI getPlayerGUI(){
        return playerGUI;
    }
}
