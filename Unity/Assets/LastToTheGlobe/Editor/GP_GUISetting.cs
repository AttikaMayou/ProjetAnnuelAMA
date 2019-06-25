using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot
public class GP_GUISettings : ScriptableObject
{
    [SerializeField]
    private bool isSpawnPlanet = false;

    [SerializeField]
    private int scale = 10;

    [SerializeField]
    private int depth = 10;

    [SerializeField]
    private PlanetType planetType;

    [SerializeField]
    private string planetName;

    public PlanetType PlanetType
    {
        get { return planetType; }
        set { planetType = value; }
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

    public string Name
    {
        get { return planetName; }
        set { planetName = value; }
    }

    public bool SpawnPlanet
    {
        get { return isSpawnPlanet; }
        set { isSpawnPlanet = value; }
    }
}


