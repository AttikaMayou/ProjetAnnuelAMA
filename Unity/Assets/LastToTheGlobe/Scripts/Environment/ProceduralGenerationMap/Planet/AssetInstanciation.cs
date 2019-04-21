using System.Collections.Generic;
using UnityEngine;

//Auteur: Margot

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet
{
    public class AssetInstanciation : MonoBehaviour
    {
        [SerializeField]
        private GameObject planet;
        [SerializeField]
        private PlanetFeature planetFt;

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

        private int numberObjectMax = 1;
        private int numberTreesMax;
        private int maxObject;
        private List<GameObject> listTrees;
        private List<GameObject> listRock;
        private float randomScaleTree;
        private float randomScaleRock;

        void Start()
        {
            Vector3 planetPosition = gameObject.transform.position;
            numberObjectMax = (int)Random.Range(1, 200);
            numberTreesMax = (int)Random.Range(0, 50);

            if (planetFt.myType == PlanetType.Frozen)
            {
                listTrees = frozenTrees;
                listRock = frozenRock;
            }
            else if (planetFt.myType == PlanetType.Desert)
            {
                listTrees = desertTrees;
                listRock = desertRock;
            }
            else if (planetFt.myType == PlanetType.Basic)
            { 
                listTrees = basicTrees;
                listRock = basicRock;
            }

            if (numberTreesMax > 0)
            {
                //Spawn Trees
                for (int i = 0; i <= numberTreesMax; i++)
                {
                    randomScaleTree = Random.Range(0.05f, 0.15f);

                    Vector3 spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + listTrees[0].transform.localScale.y - 0.02f) + planet.transform.position;
                    GameObject newtree = Instantiate(listTrees[GetRandomTree()], spawnPosition, Quaternion.identity) as GameObject;

                    newtree.transform.LookAt(planetPosition);
                    newtree.transform.localScale = new Vector3(randomScaleTree, randomScaleTree, randomScaleTree);
                    newtree.transform.Rotate(-90, 0, 0);
                    newtree.transform.parent = transform;
                }
            }

            //Spawn Rock
            for (int i = numberTreesMax; i < numberObjectMax; i++)
            {
                randomScaleRock = Random.Range(0.02f, 0.06f);

                Vector3 spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + listRock[0].transform.localScale.y - 0.05f) + planet.transform.position;
                GameObject newrock = Instantiate(listRock[GetRandomRock()], spawnPosition, Quaternion.identity) as GameObject;

                newrock.transform.LookAt(planetPosition);
                newrock.transform.localScale = new Vector3(randomScaleRock, randomScaleRock, randomScaleRock);
                newrock.transform.Rotate(-90, 0, 0);
                newrock.transform.parent = transform;
            }
        
        }

        private int GetRandomTree()
        {
            if (planetFt.myType == PlanetType.Frozen)
            {
                return (int)Random.Range(0, frozenTrees.Count - 1);
            }
            else if (planetFt.myType == PlanetType.Desert)
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
            if (planetFt.myType == PlanetType.Frozen)
            {
                return (int)Random.Range(0, frozenRock.Count - 1);
            }
            else if (planetFt.myType == PlanetType.Desert)
            {
                return (int)Random.Range(0, desertRock.Count - 1);
            }
            else
            {
                return (int)Random.Range(0, basicRock.Count - 1);
            }
        }

    }
}
