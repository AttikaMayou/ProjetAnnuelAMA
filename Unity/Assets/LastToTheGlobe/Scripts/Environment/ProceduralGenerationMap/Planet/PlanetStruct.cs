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

public struct PlanetStruct
{
    //position et radius de la planète
    public Vertex3 positionPlanet;
    public int radiusPlanet;

    //type de planète
    public PlanetType typePlanet;
    public Renderer materialPlanet;



}
