using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur: Margot

public class AssetInstanciation : MonoBehaviour
{
    [SerializeField]
    private int numberObjectMax = 1;

    [SerializeField]
    private int numberTreesMax;

    [SerializeField]
    private GameObject planet;

    [SerializeField]
    private List<GameObject> basicTrees;

    [SerializeField]
    private List<GameObject> basicRock;

    [SerializeField]
    private List<GameObject> frozenTrees;

    [SerializeField]
    private List<GameObject> frozenRock;

    [SerializeField]
    private List<GameObject> desertTrees;

    [SerializeField]
    private List<GameObject> desertRock;

    private List<GameObject> listTrees;
    private List<GameObject> listRock;
    private float randomScaleTree;
    private float randomScaleRock;

    void Start()
    {
        Vector3 planetPosition = gameObject.transform.position;

        if (planet.CompareTag("frozen"))
        {
            listTrees = frozenTrees;
            listRock = frozenRock;
        }
        else if (planet.CompareTag("desert"))
        {
            listTrees = desertTrees;
            listRock = desertRock;
        }
        else
        { 
            listTrees = basicTrees;
            listRock = basicRock;
        }

        if (numberTreesMax > 0 && numberObjectMax >0)
        {
            //Spawn Trees
            for (int i = 0; i <= numberTreesMax; i++)
            {
                randomScaleTree = Random.Range(0.05f, 0.15f);

                Vector3 spawnPosition = Random.onUnitSphere * ((gameObject.transform.localScale.x / 2) + listTrees[0].transform.localScale.y - 0.02f) + planet.transform.position;
                GameObject newtree = Instantiate(listTrees[GetRandomTree()], spawnPosition, Quaternion.identity) as GameObject;

                newtree.transform.LookAt(planetPosition);
                newtree.transform.localScale = new Vector3(randomScaleTree, randomScaleTree, randomScaleTree);
                newtree.transform.Rotate(-90, 0, 0);
                newtree.transform.parent = transform;
            }
        }

        if (numberObjectMax > 0)
        {
            //Spawn Rock
            for (int i = numberTreesMax; i < numberObjectMax; i++)
            {
                randomScaleRock = Random.Range(0.05f, 0.1f);

                Vector3 spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + listRock[0].transform.localScale.y - 0.05f) + planet.transform.position;
                GameObject newrock = Instantiate(listRock[GetRandomRock()], spawnPosition, Quaternion.identity) as GameObject;

                newrock.transform.LookAt(planetPosition);
                newrock.transform.localScale = new Vector3(randomScaleRock, randomScaleRock, randomScaleRock);
                newrock.transform.Rotate(-90, 0, 0);
                newrock.transform.parent = transform;
            }
        }
    }

    private int GetRandomTree()
    {
        if (planet.CompareTag("frozen"))
        {
            return (int)Random.Range(0, frozenTrees.Count - 1);
        }
        else if (planet.CompareTag("desert"))
        {
            return (int)Random.Range(0, desertTrees.Count - 1);
        }
        else
        {
            return (int)Random.Range(0, basicTrees.Count - 1);
        }
    }

    private int GetRandomRock()
    {
        if (planet.CompareTag("frozen"))
        {
            return (int)Random.Range(0, frozenRock.Count - 1);
        }
        else if (planet.CompareTag("desert"))
        {
            return (int)Random.Range(0, desertRock.Count - 1);
        }
        else
        {
            return (int)Random.Range(0, basicRock.Count - 1);
        }
    }

}
