using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using LastToTheGlobe.Scripts.Environment.Planets;

public class GetDataOnPlanet : MonoBehaviour
{
    public PlanetExposerScript planet;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("c'est rentrer lol");
            DataCollector.RegisterPlanet(planet);

        }
    }
}

[Serializable]
public class GetDataOnPlanetEvent : UnityEvent<GetDataOnPlanet> { };
