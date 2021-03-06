﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot
public class GP_GUISettings : ScriptableObject
{
    [SerializeField]
    private bool isSpawnPlanet = false;

    [SerializeField]
    private GameObject[] gameObjectAdd = new GameObject[200];

    [Range(0, 5)]
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

    [Range(1, 10)]
    [SerializeField]
    private float scaleRock = 1;

    [SerializeField]
    private int numberOfChest = 1;

    public PlanetType PlanetType
    {
        get { return planetType; }
        set { planetType = value; }
    }

    public GameObject[] GameObjectAdd
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

    public float ScaleRock
    {
        get { return scaleRock; }
        set { scaleRock = value; }
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

