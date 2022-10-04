using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    public PlanToSpawn[] planToSpawn; //round

    private int health; 
    private int round;
    private int nextSpawn; //라운드 별로 스폰할 적 정보 인덱스(라운드 클리어 시 초기화 해야함)
    private int enemiesByRound; //라운드에 남아 있는 적수
    //private List<GameObject> itmes; //GamaManager에서 받아올 아이템 3개

    // Start is called before the first frame update
    void Start()
    {
        InitStage();
        InitRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitStage() //1개의 스테이지에 10개의 라운드
    {
        //나중에 게임매니저에서 받아와야하는 정보들
        health = 3;
        round = 0;
    }
    public void InitHealth(int num)
    {
        this.health = num;
        Debug.Log("[StageManager]health : " + health);
    }

    void InitRound() //스테이지의 하위 항목
    {
        if (round >= planToSpawn.Length)
        {
            Debug.Log("[StageManager]Stage Clear");
            return;
        }
        foreach(var num in planToSpawn[round].numberOfEnemies)
        {
            enemiesByRound += num;
        }
        SendInfoToEnemySpawner();

    }

    public void SetHealth(int num)
    {
        this.health -= num;
        CheckGameOver();
    }

    public void CheckGameOver()
    {
        if (health <= 0)
            Debug.Log("[StageManager] : Game Over");
    }

    public void NextRound()
    {
        enemiesByRound--;
        Debug.Log("[StageManager]enemiesByRound " + enemiesByRound);
        if (enemiesByRound <= 0)
        {
            round++;
            nextSpawn = 0;//planToSpawn
            //InitRound();
            Invoke("InitRound", 5f);
            Debug.Log("[StageManager]round " + round);
        }
    }

    public void SendInfoToEnemySpawner()
    {
        if (nextSpawn >= planToSpawn[round].numberOfEnemies.Length) return; //라운드랑 스폰이랑 문제있는듯
        //Debug.Log("[SendInfoToEnemySpawner] nextSpawn " + nextSpawn);

        var enemySpawner = this.transform.GetChild(0).GetComponent<EnemySpawner>();
        enemySpawner.StartToSpawn(planToSpawn[round].kindsOfEnemy[nextSpawn], planToSpawn[round].numberOfEnemies[nextSpawn]);
        nextSpawn++;
    }

}

[Serializable]
public class PlanToSpawn
{
    public int[] kindsOfEnemy;
    public int[] numberOfEnemies;
}
