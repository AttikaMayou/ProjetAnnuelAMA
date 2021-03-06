﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

//Auteur: Margot
//Modifications : Attika

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet
{
    public class AssetInstanciation_PUN : MonoBehaviour
    {

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

        private int _numberObjectMax = 1;
        private int _numberTreesMax;
        private int _maxObject;
        private List<GameObject> _listTrees;
        private List<GameObject> _listRock;
        private float _randomScaleTree;
        private float _randomScaleRock;

        public void SpawnAssets(int numberOfTrees, int numberOfRock, PlanetType type, GameObject planet)
        {
            var planetPosition = gameObject.transform.position;
            _numberObjectMax = (int)Random.Range(1, 200);
            _numberTreesMax = (int)Random.Range(0, 50);

            switch ((PlanetType) type)
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
            }

            if (numberOfTrees > 0)
            {
                //Spawn Trees
                for (int i = 0; i <= numberOfTrees; i++)
                {
                    _randomScaleTree = Random.Range(0.05f, 0.15f);

                    var spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + _listTrees[0].transform.localScale.y - 0.02f) + planet.transform.position;
                    var newTree = PhotonNetwork.Instantiate(_listTrees[GetRandomTree(type)].name, spawnPosition, Quaternion.identity) as GameObject;

                    newTree.transform.LookAt(planetPosition);
                    newTree.transform.localScale = new Vector3(_randomScaleTree, _randomScaleTree, _randomScaleTree);
                    newTree.transform.Rotate(-90, 0, 0);
                    newTree.transform.parent = transform;
                }
            }

            //Spawn Rock
            for (var i = _numberTreesMax; i < _numberObjectMax; i++)
            {
                _randomScaleRock = Random.Range(0.02f, 0.06f);

                var spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + _listRock[0].transform.localScale.y - 0.05f) + planet.transform.position;
                var newRock = PhotonNetwork.Instantiate(_listRock[GetRandomRock(type)].name, spawnPosition, Quaternion.identity) as GameObject;

                newRock.transform.LookAt(planetPosition);
                newRock.transform.localScale = new Vector3(_randomScaleRock, _randomScaleRock, _randomScaleRock);
                newRock.transform.Rotate(-90, 0, 0);
                newRock.transform.parent = transform;
            }
        }

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

    }
}
