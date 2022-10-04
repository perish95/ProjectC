using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBoard : MonoBehaviour
{
    public GameObject board;
    private GameObject[] boardToChildren; //row, column, padding도 필요할듯
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("SortingBoard " + board.transform.childCount);
        //boardToChildren = GetChild(board);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
