using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorType : MonoBehaviour
{
    public int red { get; set; }
    public int green { get; set; } 
    public int blue { get; set; }
    public ColorType(int r, int g, int b)
    {
        red = r;
        green = g;
        blue = b;
    }

    public ColorType() //Default
    {
        red = 0;
        green = 0;
        blue = 0;
    }

    public ColorType(ColorType a, ColorType b)
    {
        this.red = a.red + b.red;
        this.green = a.green + b.green;
        this.blue = a.blue + b.blue;
    }

    /*public void MergeColor(ColorType a, ColorType b)
    {
        this.red = a.red + b.red;
        this.green = a.green + b.green;
        this.blue = a.blue + b.blue;
        Debug.Log("ColorType : " + red + " " + blue + " " + green);
    }*/

    public Color GetColorType()
    {
        float R = (red == 0) ? 0 : 255 / red;
        float G = (green == 0) ? 0 : 255 / green;
        float B = (blue == 0) ? 0 : 255 / blue;

        // 같은 색을 바꿀 경우 다른 이펙트가 보이도록 해야겠다.

        return new Color(R, G, B);
    }
}
