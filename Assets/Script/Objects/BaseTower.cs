using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [Header("Attribute")]
    //public int attack; //이건 총알에 있어야 할 속성이네
    public int range;
    public int level;
    public float fireRate; //총알 발사 주기(초당)
    public GameObject bulletPrefab;
    public float atk; //공격력

    private float fireCountdwon = 0f;
    private Transform target; //타켓이 잡히는 지 확인용
    private ColorType towerColor;
    private Vector3 mousePos;
    private Vector3 originPos;
    private float mousePosZ;
    private bool grap = false;
    private bool merge = false;
    private Ground ground;
    private BaseTower otherTower; //타워끼리 합쳐졌을 때 임시로 담을 변수

    void Start()
    {
        if(level == 1)
            SetColor();
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //0초에 시작, 0.5초마다 반복
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;
        if (fireCountdwon <= 0f && !grap)
        {
            Shoot(); //발사 속도를 조절하고, 총알의 색도 타워의 색과 똑같게 입혀야 한다.
            fireCountdwon = 1f / fireRate;
        }fireCountdwon -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        //if (grap) return; //마우스로 클릭 시에 총알 발사 금지

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDis = Mathf.Infinity;
        GameObject targetEnemy = null;
        
        foreach (GameObject e in enemies)
        {
            float distanceToEnemy = Vector2.Distance(this.transform.position, e.transform.position); //거리 비교는 무조건 Vector2로

            if (distanceToEnemy < shortestDis)
            {
                shortestDis = distanceToEnemy;
                targetEnemy = e;
            }
        }

        if (targetEnemy != null && shortestDis <= range)
        {
            target = targetEnemy.transform;
        }
        else
            target = null;

    }

    public void SetGround(Ground g)
    {
        ground = g;
    }

    private void Shoot()
    {
        GameObject tmp = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = tmp.GetComponent<Bullet>();
        
        bullet.SetColor(towerColor);
        bullet.SetAtk(atk);
        bullet.SeekTarget(target);
    }

    private void SetColor()
    {
        int tmp = Random.Range(0, 3);
        switch (tmp)
        {
            case 0:
                this.towerColor = new ColorType(1, 0 ,0);
                break;
            case 1:
                this.towerColor = new ColorType(0, 1, 0);
                break;
            case 2:
                this.towerColor = new ColorType(0, 0, 1);
                break;
            default:
                break;
        }
        var circle = this.gameObject.transform.GetChild(0).gameObject;
        circle.GetComponent<SpriteRenderer>().material.color = towerColor.GetColorType();
    }

    public void SetColor(ColorType colorType)
    {
        this.towerColor = colorType;
        var circle = this.gameObject.transform.GetChild(0).gameObject;
        circle.GetComponent<SpriteRenderer>().material.color = colorType.GetColorType();
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnMouseDown()
    {
        grap = true;
        originPos = this.transform.position;
        mousePosZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mousePos = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        grap = true;
        transform.position = GetMouseWorldPosition() + mousePos;
    }

    private void OnMouseUp()
    {
        if (!merge)
            this.transform.position = originPos;
        else
        {
            MergeTower(otherTower.transform.position, otherTower.towerColor, otherTower.ground);
            otherTower.DestroyTower();
        }
        grap = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mousePosZ;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tower" && grap)
        {
            otherTower = collision.gameObject.GetComponent<BaseTower>();
            if (otherTower.level == this.level)
            {
                if (level >= 3)
                {
                    otherTower = null;
                    return;
                }

                merge = true;
            }
            else
            {
                otherTower = null;
                return;
            }
        }
    }

    private void MergeTower(Vector3 pos, ColorType colorType, Ground other)
    {
        //[BUG]에너미 스포너랑 제일 가까운 땅에서 타워가 잘 안잡히는 현상
        ColorType tmp = new ColorType(colorType, this.towerColor);

        GameObject.Find("TowerSpawner").GetComponent<SpawnTower>().MergedTower(this.level+1, pos, tmp, other);
        ground.RemoveTower();
        DestroyTower();
    }

    public void DestroyTower()
    {
        Destroy(gameObject);
    }
}
