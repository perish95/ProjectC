using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies; // <적의 종류, 스폰할 수> 이런 식으로 구조를 만들어서 코루틴을 돌리면 좋을 것 같다.
    
    private int toSpawnCount; //스폰할 에너미 수
    
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

            yield break; //코루틴을 바로 빠져나옴, 한 라운드 종료
        }

        Instantiate(enemies[enemiesIndex], this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2.0f);

        StartCoroutine(SpawnEnemy(currentNumber+1, enemiesIndex));
    }
}
