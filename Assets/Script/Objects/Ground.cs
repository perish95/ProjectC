using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool isBuilt;
    
    void Start()
    {
        isBuilt = false;
    }

    public void BuildTower(){ isBuilt = true; }

    public void RemoveTower() { isBuilt = false; }
    
    public bool GetisBuilt() { return isBuilt; } 
}
