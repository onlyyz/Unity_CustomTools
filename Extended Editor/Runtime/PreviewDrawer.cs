using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : PropertyAttribute
{
    public int height;
    public int weight;
    public Preview()
    {
        
    }
    public Preview(int hei, int wei)
    {
        this.height = hei;
        this.weight = weight;
    }
}