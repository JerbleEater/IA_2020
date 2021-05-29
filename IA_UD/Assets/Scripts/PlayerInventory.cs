using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Might have to figure out how to make player inventory serializable 
public class PlayerInventory : MonoBehaviour
{
    // Private Vars
    private Dictionary<Item.ItemType, int> itemDict; 

    public void Awake(){
        itemDict = new Dictionary<Item.ItemType, int>();
    }
    
    // Getters
    public Dictionary<Item.ItemType, int> getItemDict(){
        return itemDict;
    }
}
