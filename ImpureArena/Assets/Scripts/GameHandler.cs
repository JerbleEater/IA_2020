using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Public Vars
    public static GameHandler Instance{get{return _instance;}}
    public Player player;
    public EnemySpawner enemySpawner;

    // Private Vars
    private static GameHandler _instance;

    // Singleton
    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else{
            _instance = this;
        }
    }
    
    public void fuckEnemyUp(SwipeGesture.Direction userDirection, float damage){
        enemySpawner.checkInSpawnedEnemyList(userDirection, damage);
    }
    
    public void fuckPlayerOneUp(){
        player.decreaseInGameHp();
    }

    // Populate Player Database Use in multiplayer
    /*private void addPlayerToPlayerDatabase(Player newPlayer, string goName = ""){
        if(goName != ""){
            playerDatabase.Add(GameObject.Find(goName).GetComponent<Player>());
            return;
        }
        playerDatabase.Add(newPlayer);
    } */  

}
