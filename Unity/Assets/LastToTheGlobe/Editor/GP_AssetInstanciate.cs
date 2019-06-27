using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

//Auteur: Margot
//Modifications : Attika

namespace LastToTheGlobe.Editor
{
    public class GP_AssetInstanciate : MonoBehaviour 
    {

       /* [SerializeField]
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
        */
        private GameObject newTree;
        private GameObject newRock;
        private int randomBasicTrees;
        private int randomRock;
        private int _numberObjectMax = 1;
        private int _numberTreesMax;
        private int _maxObject;
        ///private List<GameObject> _listTrees;
        //private List<GameObject> _listRock;
        private float _randomScaleTree;
        private float _randomScaleRock;

        public void CreateAssets(int numberOfTrees, int numberOfRock, PlanetType type, GameObject planet)
        {
            _numberObjectMax = (int)Random.Range(1, 200);
            _numberTreesMax = (int)Random.Range(0, 50);
/*
            switch ((PlanetType)type)
            {
                case PlanetType.Frozen:
                    _listTrees = frozenTrees;
                    _listRock = frozenRock;
                    break;
                case PlanetType.Desert:
                    _listTrees = desertTrees;
                    _listRock = desertRock;
                    break;
                case PlanetType.Basic:
                    _listTrees = basicTrees;
                    _listRock = basicRock;
                    break;
                default:
                    _listTrees = basicTrees;
                    _listRock = basicRock;
                    break;
            }*/
           
            if (numberOfTrees > 0)
            {
                //Spawn Trees
                for (int i = 1; i <= numberOfTrees; i++)
                {
                    _randomScaleTree = Random.Range(0.05f, 0.15f);
                    randomBasicTrees = Random.Range(1, 4);

                    newTree = Instantiate(Resources.Load("SM_Basic_Tree_0" + randomBasicTrees, typeof(GameObject))) as GameObject;
                    var spawnPosition = Random.onUnitSphere * (((planet.transform.localScale.x / 2) - 0.2f) + newTree.transform.localScale.y - 0.1f) + planet.transform.position;
                    newTree.transform.position = spawnPosition;
                    newTree.transform.LookAt(planet.transform.position);
                    newTree.transform.localScale = new Vector3(_randomScaleTree, _randomScaleTree, _randomScaleTree);
                    newTree.transform.Rotate(-90, 0, 0);
                    //newTree.transform.parent = transform;
                }
            }
            /*
            //Spawn Rock
            for (var i = _numberTreesMax; i < _numberObjectMax; i++)
            {
                randomRock = Random.Range(1, 4);
                _randomScaleRock = Random.Range(0.02f, 0.06f);

                newRock = Instantiate(Resources.Load("P_RockDesert_v" + randomRock, typeof(GameObject))) as GameObject;
                var spawnPosition = Random.onUnitSphere * (((planet.transform.localScale.x / 2) - 0.2f) + newTree.transform.localScale.y - 0.1f) + planet.transform.position;

                newRock.transform.LookAt(planetPosition);
                newRock.transform.localScale = new Vector3(_randomScaleRock, _randomScaleRock, _randomScaleRock);
                newRock.transform.Rotate(-90, 0, 0);
                //newRock.transform.parent = transform;
            }*/
        }
        /*
        private int GetRandomTree(PlanetType type)
        {
            switch ((PlanetType)type)
            {
                case PlanetType.Frozen:
                    return (int)Random.Range(0, frozenTrees.Count - 1);
                case PlanetType.Desert:
                    return (int)Random.Range(0, desertTrees.Count - 1);
                case PlanetType.Basic:
                    break;
                default:
                    return (int)Random.Range(0, basicTrees.Count - 1);
            }

            return 0;
        }

        private int GetRandomRock(PlanetType type)
        {
            switch ((PlanetType)type)
            {
                case PlanetType.Frozen:
                    return (int)Random.Range(0, frozenRock.Count - 1);
                case PlanetType.Desert:
                    return (int)Random.Range(0, desertRock.Count - 1);
                case PlanetType.Basic:
                    break;
                default:
                    return (int)Random.Range(0, basicRock.Count - 1);
            }

            return 0;
        }
        */
    }
}
