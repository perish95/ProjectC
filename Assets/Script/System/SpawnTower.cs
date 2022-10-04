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
    public GameObject board; //����� Game Manager���� �����ؾ��� ������Ʈ�� �����ؾ��� ��
    public int builtCost; //Ÿ�� ���� ���
    public int curCost; //�������� �ʱ⿡ �־����� ��ȭ
    public TextMeshProUGUI costText;
    public delegate void CostDelegate(int num);
    public CostDelegate costDelegate; //Cost Text ������ ���� ��������Ʈ

    private Ground[] grounds;
    private SortedSet<int> built = new SortedSet<int>(); //stage manager�� ���� �����ϴ� �� �����غ���
    private int cntTower; //�ʿ� ��ġ�� Ÿ���� ��
    //private StageManager stageManager;

    void Start()
    {
        grounds = board.GetComponentsInChildren<Ground>(); //GetComponentsInChildren�� �θ�ü�� ��´�.
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

    private Ground SelectGround() //List�� Ground�� ����ִ� ������ ���ٷ� �����ϸ� �������ϴ�.
    {
        List<Ground> tmp = new List<Ground>(); // �� �κ��� ���ٷ� �ϸ� �ڵ尡 ����� �� ����.
        int idx; //������ ���� �ε���

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
