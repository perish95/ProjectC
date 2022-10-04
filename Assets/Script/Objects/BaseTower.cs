using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [Header("Attribute")]
    //public int attack; //�̰� �Ѿ˿� �־�� �� �Ӽ��̳�
    public int range;
    public int level;
    public float fireRate; //�Ѿ� �߻� �ֱ�(�ʴ�)
    public GameObject bulletPrefab;
    public float atk; //���ݷ�

    private float fireCountdwon = 0f;
    private Transform target; //Ÿ���� ������ �� Ȯ�ο�
    private ColorType towerColor;
    private Vector3 mousePos;
    private Vector3 originPos;
    private float mousePosZ;
    private bool grap = false;
    private bool merge = false;
    private Ground ground;
    private BaseTower otherTower; //Ÿ������ �������� �� �ӽ÷� ���� ����

    void Start()
    {
        if(level == 1)
            SetColor();
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //0�ʿ� ����, 0.5�ʸ��� �ݺ�
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;
        if (fireCountdwon <= 0f && !grap)
        {
            Shoot(); //�߻� �ӵ��� �����ϰ�, �Ѿ��� ���� Ÿ���� ���� �Ȱ��� ������ �Ѵ�.
            fireCountdwon = 1f / fireRate;
        }fireCountdwon -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        //if (grap) return; //���콺�� Ŭ�� �ÿ� �Ѿ� �߻� ����

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDis = Mathf.Infinity;
        GameObject targetEnemy = null;
        
        foreach (GameObject e in enemies)
        {
            float distanceToEnemy = Vector2.Distance(this.transform.position, e.transform.position); //�Ÿ� �񱳴� ������ Vector2��

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
        //[BUG]���ʹ� �����ʶ� ���� ����� ������ Ÿ���� �� �������� ����
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
