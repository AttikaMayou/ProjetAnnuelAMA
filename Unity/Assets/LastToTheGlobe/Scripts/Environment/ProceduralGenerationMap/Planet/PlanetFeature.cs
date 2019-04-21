using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFeature : MonoBehaviour
{
    [SerializeField]
    private GameObject planet;
    //Planet tag

    string[] tags = new string[] { "basic", "frozen", "desert" };

    private float scale;
    private int indexRandom;

    //TODO : transformer planet en prefab

    private void Start()
    {
        //Planet tag
        indexRandom = (int)Random.Range(0, 3);
        planet.tag = tags[indexRandom];
        
        //Size Planet
        scale = Random.Range(20f, 50f);
        planet.transform.localScale = new Vector3(scale, scale, scale);

        //Planet Material
        if (planet.CompareTag("frozen"))
        {
            Material matFrozen = Resources.Load("M_FrozenPlanet", typeof(Material)) as Material;
            planet.GetComponent<Renderer>().material = matFrozen;
        }
        else if (planet.CompareTag("desert"))
        {
            Material matdesert = Resources.Load("M_DesertPlanet", typeof(Material)) as Material;
            planet.GetComponent<Renderer>().material = matdesert;
        }
        else
        {
            Material matBasic = Resources.Load("M_BasicPlanet", typeof(Material)) as Material;
            planet.GetComponent<Renderer>().material = matBasic;
        }


    }

}
