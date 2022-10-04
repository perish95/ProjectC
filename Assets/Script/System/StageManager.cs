using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    public PlanToSpawn[] planToSpawn; //round

    private int health; 
    private int round;
    private int nextSpawn; //���� ���� ������ �� ���� �ε���(���� Ŭ���� �� �ʱ�ȭ �ؾ���)
    private int enemiesByRound; //���忡 ���� �ִ� ����
    //private List<GameObject> itmes; //GamaManager���� �޾ƿ� ������ 3��

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

    private void InitStage() //1���� ���������� 10���� ����
    {
        //���߿� ���ӸŴ������� �޾ƿ;��ϴ� ������
        health = 3;
        round = 0;
    }
    public void InitHealth(int num)
    {
        this.health = num;
        Debug.Log("[StageManager]health : " + health);
    }

    void InitRound() //���������� ���� �׸�
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
        if (nextSpawn >= planToSpawn[round].numberOfEnemies.Length) return; //����� �����̶� �����ִµ�
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
