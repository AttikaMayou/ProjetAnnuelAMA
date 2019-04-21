using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPInstanciation : MonoBehaviour
{
    [SerializeField]
    private GameObject tremplin;

    [SerializeField]
    private GameObject chest;

    [SerializeField]
    private GameObject planet;

    private int numberOfTremplin;
    private int numberOfChest;

    void Start()
    {
        numberOfTremplin = Random.Range(1, 5);
        numberOfChest = Random.Range(1, 3);
        //TODO : changer en fonction de la taille des planètes
        // faire en sorte qu'ils soient toujours vers d'autres plantètes

        for (int i = 0; i <= numberOfTremplin; i++)
        {
            Vector3 planetPosition = gameObject.transform.position;

            Vector3 spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + tremplin.transform.localScale.y - 0.02f) + planet.transform.position;
            GameObject newtree = Instantiate(tremplin, spawnPosition, Quaternion.identity) as GameObject;

            newtree.transform.LookAt(planetPosition);
            newtree.transform.Rotate(-90, 0, 0);
            newtree.transform.parent = transform;
        }

        for (int i = 0; i <= numberOfChest; i++)
        {
            Vector3 planetPosition = gameObject.transform.position;

            Vector3 spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + chest.transform.localScale.y - 0.02f) + planet.transform.position;
            GameObject newtree = Instantiate(chest, spawnPosition, Quaternion.identity) as GameObject;

            newtree.transform.LookAt(planetPosition);
            newtree.transform.Rotate(-90, 0, 0);
            newtree.transform.parent = transform;
        }
    }


}
