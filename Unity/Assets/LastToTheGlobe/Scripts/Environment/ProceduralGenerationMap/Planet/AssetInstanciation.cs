using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur: Margot

public class AssetInstanciation : MonoBehaviour
{
    [SerializeField]
    private GameObject tree;

    [SerializeField]
    private int numberObjectsMax;

    [SerializeField]
    private GameObject planet;

    void Start()
    {
        Vector3 planetPosition = gameObject.transform.position;
        Vector3 spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + tree.transform.localScale.y - 0.02f) + planet.transform.position;

        for (int i = 0; i < numberObjectsMax; i++)
        {
            GameObject newtree = Instantiate(tree, spawnPosition, Quaternion.identity) as GameObject;
            newtree.transform.LookAt(planetPosition);
            newtree.transform.Rotate(-90, 0, 0);
        }
    }


        // Update is called once per frame
        void Update()
    {

    }
}
