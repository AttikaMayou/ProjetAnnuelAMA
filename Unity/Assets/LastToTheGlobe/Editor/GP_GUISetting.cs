﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot
public class GP_GUISettings : ScriptableObject
{
    [SerializeField]
    private bool isSpawnPlanet = false;

    [SerializeField]
    private List<Object> gameObjectAdd = new List<Object>();

    [SerializeField]
    private int numberOfgameObjectAdd;

    [SerializeField]
    private int scale = 10;

    [SerializeField]
    private PlanetType planetType;

    [SerializeField]
    private string planetName;

    [SerializeField]
    private int numberOfTree = 0;

    [SerializeField]
    private int numberOfRock = 0;

    [SerializeField]
    private int numberOfChest = 1;

    public PlanetType PlanetType
    {
        get { return planetType; }
        set { planetType = value; }
    }

    public List<Object> GameObjectAdd
    {
        get { return gameObjectAdd; }
        set { gameObjectAdd = value; }
    }

    public int Scale
    {
        get { return scale; }
        set { scale = value; }
    }

    public int NumberOfgameObjectAdd
    {
        get { return numberOfgameObjectAdd; }
        set { numberOfgameObjectAdd = value; }
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

    public int NumberOfTree
    {
        get { return numberOfTree; }
        set { numberOfTree = value; }
    }

    public int NumberOfRock
    {
        get { return numberOfRock; }
        set { numberOfRock = value; }
    }

    public int NumberOfChest
    {
        get { return numberOfChest; }
        set { numberOfChest = value; }
    }
}

