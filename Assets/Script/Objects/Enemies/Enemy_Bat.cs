using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : Enemy
{
    public Animator anim;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    protected override void DeathEvent()
    {
        GameObject.Find("StageManager").GetComponent<StageManager>().NextRound();
        anim.SetBool("Death",true);
        Invoke("DestroyBat", 0.7f);
    }

    void DestroyBat()
    {
        Destroy(gameObject);
    }
}
