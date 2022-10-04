using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    private Transform target;
    private Enemy tmp;
    private float towerAtk;
    private ColorType bulletColor;
    
    void Start(){ }

    public void SeekTarget(Transform _target)
    {
        target = _target;
        tmp = target.gameObject.GetComponent<Enemy>();
    }

    public void SetColor(ColorType towerColor)
    {
        bulletColor = towerColor;
        gameObject.GetComponent<Renderer>().material.color = bulletColor.GetColorType();
    }

    public void SetAtk(float atk) { towerAtk = atk; }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime; 

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame);
    }

    private void HitTarget()
    {
        
        tmp.Attacked(towerAtk, bulletColor);
        Destroy(gameObject);
    }
}   

