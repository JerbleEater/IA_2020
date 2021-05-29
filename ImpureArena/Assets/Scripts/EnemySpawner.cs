using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Public Vars
    public Enemy enemyPrefab;
    public bool enemySpawnerOn;

    //Private Vars
    private List<Enemy> spawnedEnemyList;

    //Called without enabling script
    private void Awake(){
        spawnedEnemyList = new List<Enemy>();
    }

    //Called when script is enabled
    private void Start(){
        if(enemySpawnerOn){
            StartCoroutine(generateEnemy(2f));
        }
    }

    /*
        Function: Generate Enemy(Coroutine), Params: float
        This function generates an enemy per the specified delay.
        The enemy will be spawned in one of three positions.
    */
    private IEnumerator generateEnemy(float delayInSeconds){
        
        yield return new WaitForSeconds(delayInSeconds);

        removeNullSpacesFromDatabase();

        int randomNum = Random.Range(0, 3);
        
        switch(randomNum){
            case 0:
                spawnedEnemyList.Add(Instantiate(enemyPrefab, new Vector3(-99.7f, 55f, 649f), Quaternion.identity));
                spawnedEnemyList[spawnedEnemyList.Count - 1].GetComponentInChildren<SpriteRenderer>().flipX = true;
                break;
            case 1:
                spawnedEnemyList.Add(Instantiate(enemyPrefab, new Vector3(45.3f, 55f, 649f), Quaternion.identity));
                break;
            case 2:
                spawnedEnemyList.Add(Instantiate(enemyPrefab, new Vector3(-29.7f, 55f, 649f), Quaternion.identity));
                break;
            default:
                break;
        }
        StartCoroutine(generateEnemy(delayInSeconds));
    }

    /*
        Function: Remove Null Spaces from DB, Params: None
        This function removes all spaces that are missing references to enemy objs
        in the enemy database. Helps ensure memory is utilized properly.
    */
    private void removeNullSpacesFromDatabase(){
        spawnedEnemyList.RemoveAll(Enemy => Enemy == null);
    }

    /*
        Function: Check in Spawned Enemy List, Params: Swipe Gesture, Float
        This function passes the users swipe direction and users damage to
        to each enemys current direction checker in the enemy database. 
    */
    public void checkInSpawnedEnemyList(SwipeGesture.Direction userDirection, float damage){
        foreach(Enemy enemy in spawnedEnemyList){
            if(enemy != null){
                enemy.checkCurrentDirection(userDirection, damage);
            } 
        }
    }
}
