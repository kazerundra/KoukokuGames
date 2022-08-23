using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{

    public CharacterMovement enemy;
    public Transform stopPos, enemyPos;
    public GameObject EnemySprite;
    public int towerNumber;
    public bool clear = false;

    private void Awake()
    {
        stopPos = this.transform.GetChild(0).transform;
        enemyPos = this.transform.GetChild(1).transform;
        GameObject go = Instantiate(EnemySprite, enemyPos, true);
        go.transform.position = enemyPos.transform.position;
     
        enemy = go.GetComponent<CharacterMovement>();
        enemy.FlipChar();
        enemy.currentTower = this;
        
    }


}
