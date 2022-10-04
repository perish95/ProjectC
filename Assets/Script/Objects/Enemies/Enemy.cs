using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    public int toCost; //주는 비용

    protected ColorType enemyColor;
    protected Transform target;
    protected int wavepointIdx = 0;
    protected void Start()
    {
        target = Waypoints.points[0];
        SetColor();
    }

    // Update is called once per frame
    protected void Update()
    {
        Vector3 dir = target.position - this.transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }

        /*if(health <= 0f)
        {
            GameObject.Find("TowerSpawner").GetComponent<SpawnTower>().costDelegate(toCost); 
            DeathEvent();
            Destroy(gameObject);//TODO 오브젝트가 사라지지 않아서 계속 호출하면서 생기는 문제; 데스모션이랑 타이밍 맞는 구간을 찾아야 할듯
            return;
        }*/
    }

    private void GetNextWaypoint()
    {
        if(wavepointIdx >= Waypoints.points.Length - 1)
        {
            AttackPlayer();
            DeathEvent();
            Destroy(gameObject);
            return;
        }

        wavepointIdx++;
        target = Waypoints.points[wavepointIdx];
    }

    private void SetColor()
    {
        int tmp = Random.Range(0, 3);
        switch (tmp)
        {
            case 0:
                this.enemyColor = new ColorType(1, 0, 0);
                break;
            case 1:
                this.enemyColor = new ColorType(0, 1, 0);
                break;
            case 2:
                this.enemyColor = new ColorType(0, 0, 1); // new Color(float, float, float);
                break;
            default:
                break;
        }
        var circle = this.gameObject.transform.GetChild(0).gameObject;
        circle.GetComponent<SpriteRenderer>().material.color = enemyColor.GetColorType();
    }

    virtual protected void DeathEvent()
    {
    }

    private void AttackPlayer()
    {
        var stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageManager.SetHealth(1);
    }

    public void Attacked(float dmg, ColorType bulletColor)
    {
        if (health < 0f) return;
        if (enemyColor.red > 0 &&  bulletColor.red > 0)
        {
            dmg += (dmg * 0.3f * bulletColor.red);
        }
        else if(enemyColor.green > 0 && bulletColor.green > 0)
        {
            dmg += (dmg * 0.3f * bulletColor.green);
        }
        else if(enemyColor.blue > 0 && bulletColor.blue > 0)
        {
            dmg += (dmg * 0.3f * bulletColor.blue);
        }
        else
        {
            dmg -= (dmg * 0.3f);
        }
        this.health -= dmg;

        if(health <= 0f)
        {
            GameObject.Find("TowerSpawner").GetComponent<SpawnTower>().costDelegate(toCost);
            DeathEvent();
        }
    }
}
