using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using System;

public class SpawnTower : MonoBehaviour
{
    [Header("Element")]
    public GameObject[] towers;
    public GameObject board; //보드는 Game Manager에서 관리해야할 오브젝트로 변경해야할 듯
    public int builtCost; //타워 생성 비용
    public int curCost; //스테이지 초기에 주어지는 재화
    public TextMeshProUGUI costText;
    public delegate void CostDelegate(int num);
    public CostDelegate costDelegate; //Cost Text 갱신을 위한 델리게이트

    private Ground[] grounds;
    private SortedSet<int> built = new SortedSet<int>(); //stage manager을 만들어서 관리하는 걸 생각해보자
    private int cntTower; //맵에 설치된 타워의 수
    //private StageManager stageManager;

    void Start()
    {
        grounds = board.GetComponentsInChildren<Ground>(); //GetComponentsInChildren는 부모객체도 담는다.
        cntTower = 0;
        costText.text = curCost.ToString();
        costDelegate += AddCost;
        costDelegate += SetCostText;
        //stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    void Update() { }
    
    public void SpawnBaseTower()
    {
        if (curCost - builtCost < 0)
        {
            Debug.Log("[Spawntower] not enough cost " + curCost + " " + builtCost );
            return;
        }

        if(cntTower == grounds.Length)
        {
            Debug.Log("[SpawnTower] : dont build tower");
            return;
        }

        Ground ground = SelectGround();
        Vector3 pos = new Vector3(ground.transform.position.x, ground.transform.position.y, ground.transform.position.z);

        GameObject tmp = Instantiate(towers[0], pos, Quaternion.identity);    
        tmp.GetComponent<BaseTower>().SetGround(ground);
        cntTower++;
        costDelegate(-builtCost);
    }

    public void MergedTower(int level,Vector3 pos, ColorType color, Ground ground)
    {
        if(level >= 3)
        {
            Debug.Log("SpawnTower : dont upgrade tower" + level);
        }
        GameObject tmp = Instantiate(towers[level-1], pos, Quaternion.identity);
        tmp.GetComponent<BaseTower>().SetColor(color);
        tmp.GetComponent<BaseTower>().SetGround(ground);
        cntTower--;
    }

    private Ground SelectGround() //List에 Ground를 집어넣는 과정을 람다로 수정하면 좋을듯하다.
    {
        List<Ground> tmp = new List<Ground>(); // 이 부분을 람다로 하면 코드가 깔끔할 것 같다.
        int idx; //선택할 땅의 인덱스

        foreach (Ground g in grounds)
        {
            if (!g.GetisBuilt()) tmp.Add(g);
        }

        idx = Random.Range(0, tmp.Count);
        tmp[idx].BuildTower();

        return tmp[idx];
    }

    public void AddCost(int num)
    {
        curCost += num;
    }
    public void SetCostText(int num)
    {
        costText.text = curCost.ToString();
    }
}
