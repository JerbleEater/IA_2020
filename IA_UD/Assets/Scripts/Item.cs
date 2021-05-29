using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour{   
    // Public Vars
    public enum ItemType{Heart, Grenade, Nuke, AutoTurrent, GoldenHearts, Damage, CritChance, InstaKillMF, MoMoney};
    public enum CurrentState{OnFloor, InInventory};
    
    // Private Vars
    private ItemType itemType;
    private CurrentState itemState;
    private float damageAdded, critChanceAdded, lerpTime;
    private bool followMousePos;
    private Player player;

    private void Awake(){
        /* I should rethink about doing this, the item should be added to the inventory of
        the player who clicked on the item. This only works in a single player game
        not multiplayer game, as there would be more than one main player. 
        */
        player = GameObject.Find("MainPlayer").GetComponent<Player>();
    }

    private void FixedUpdate(){
        if(followMousePos){
            var mousePos = Input.mousePosition;
            mousePos.z = 1f;
            this.GetComponent<RectTransform>().position = mousePos;
        } 
    }

    /*
        Function: Item Action, Params: None
        This function uses a switch statement to determine what the item should do
        based on the items type. 
    */
    private void itemAction(){
        switch(itemType){
            case ItemType.Heart:
                player.increaseInGameHp();
                break;
            case ItemType.Grenade:
                break;
            default:
                break;
        }
    }

    /*
        Function: Item Clicked(Event), Params: None
        This function is triggered when an item is clicked. It checks the state of the 
        item(OnFloor || InInventory). A switch statement is used to determine if the item 
        should be added to the player inventory or removed. In the instance that the item 
        is removed, the followMousePos bool is checked on.
    */
    private void itemClicked(){
        switch(itemState){
            case CurrentState.OnFloor:
                player.addItemToInventory(this.gameObject, this.itemType);
                break;
            case CurrentState.InInventory:
                player.removeItemFromInventory(this.gameObject, this.itemType);
                followMousePos = true;
                break;
        }
    }

    /*
        Function: Item Unclicked(Event), Params: None
        This function is triggered when an item is unclicked. It sets the followMousePos off. 
        If the pos of the item when unclicked is below the specified y pos, it is added back to 
        the player inventory, otherwise the items action is executed and the item is destroyed.
        Items are set to InInventory state if they are not destroyed.
    */
    private void itemUnclicked(){
        followMousePos = false;

        RectTransform itemRT = this.gameObject.GetComponent<RectTransform>();

        if(itemRT.localPosition.y < -450f){
            player.addItemToInventory(this.gameObject, this.itemType);
        } else if(itemState == CurrentState.InInventory){
            itemAction();
            Destroy(this.gameObject);
        }

        itemState = CurrentState.InInventory;

    }

    // Setters
    public void setItemState(CurrentState itemState){
        this.itemState = itemState;
    }

    // Getters
    public float getItemDamageAdded(){
        return damageAdded;
    }

    public float getItemCritChanceAdded(){
        return critChanceAdded;
    }

}
