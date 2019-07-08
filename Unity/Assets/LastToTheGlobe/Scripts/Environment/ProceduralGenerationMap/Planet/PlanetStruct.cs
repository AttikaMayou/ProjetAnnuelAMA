using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi;

public enum PlanetType
{
    Basic,
    Frozen,
    Desert
}

public enum AssetType
{
    Tree,
    Rock
}

class PlanetClass
{
    public int planetID;

    //position et radius de la planète
    public Vector3 planetLocation;
    public float radiusPlanet;

    public GameObject gameObjectPlanet;

    //type de planète
    public PlanetType planetType;
    public Renderer planetMaterial;

    // assets sur cette planète
    public AssetStruct[] planetAssets;
    //TODO : ajout fonction set scale + fonction set material

}

public struct AssetStruct
{
    // position et type de l'asset
    public AssetType assetType; // tree or rock
    public int meshTypeID; // index in the list
    public Vector3 relativeLocation; // location on the planet

}