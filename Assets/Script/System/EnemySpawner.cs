using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies; // <���� ����, ������ ��> �̷� ������ ������ ���� �ڷ�ƾ�� ������ ���� �� ����.
    
    private int toSpawnCount; //������ ���ʹ� ��
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartToSpawn(int enemiesIndex, int count)
    {
        toSpawnCount = count;
        StartCoroutine(SpawnEnemy(0, enemiesIndex));

    }

    IEnumerator SpawnEnemy(int currentNumber, int enemiesIndex)
    {
        if (toSpawnCount == currentNumber)
        {
            toSpawnCount = 0;
            StopAllCoroutines();
            this.transform.GetComponentInParent<StageManager>().SendInfoToEnemySpawner();

            yield break; //�ڷ�ƾ�� �ٷ� ��������, �� ���� ����
        }

        Instantiate(enemies[enemiesIndex], this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2.0f);

        StartCoroutine(SpawnEnemy(currentNumber+1, enemiesIndex));
    }
}
