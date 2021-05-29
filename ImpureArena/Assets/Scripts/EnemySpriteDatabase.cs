using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "ImpureArena/EnemyDatabase", order = 0)]
public class EnemySpriteDatabase : ScriptableObject {
    /* !!!!!!!! IMPORTANT NOTE !!!!!!!!
        If any changes are made to the variable names(enemySpritePrefab & enemyHealth),
        all prefabs or values designated to that variable will be removed, and will need
        to be reassigned again. 
    */
    [System.Serializable]
    public struct EnemyNode{
        [SerializeField]
        public GameObject enemySpritePrefab;
        [SerializeField]
        public float enemyHealth;
    }

    public List<EnemyNode> enemyList = new List<EnemyNode>();
    
    /*
        Function: Random Enemy Selector, Params: Transform, Return: Float
        This function randomly chooses an enemy sprite, and the selected enemy sprite transform
        is connected to the new enemy. The designated health of the selected enemy sprite
        is returned. 
    */
    public float randomEnemySelector(Transform parentTransform){
        int rand = Random.Range(0, enemyList.Count);

        GameObject sprite = Instantiate(enemyList[rand].enemySpritePrefab, parentTransform);
        sprite.transform.localPosition = new Vector3(-542.96f, -1060.34f, 11.67f);
        sprite.transform.localScale    = new Vector3(30f, 30f, 30f);

        return enemyList[rand].enemyHealth;
    }

}
