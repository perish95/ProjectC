using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Camera camara = GetComponent<Camera>();
        Rect rect = camara.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)16 / 9); //(가로 / 세로)
        float scaleWidth = 1f / scaleHeight;

        if(scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }
        camara.rect = rect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
