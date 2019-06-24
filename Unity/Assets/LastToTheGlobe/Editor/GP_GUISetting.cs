using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP_GUISettings : ScriptableObject
{
    [SerializeField]
    private int scale = 10;

    [SerializeField]
    private int depth = 10;

    [SerializeField]
    private float digRate = 0.3f;

    public float DigRate
    {
        get { return digRate; }
        set { digRate = value; }
    }

    public int Depth
    {
        get { return depth; }
        set { depth = value; }
    }

    public int Scale
    {
        get { return scale; }
        set { scale = value; }
    }
}


