﻿using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Margot

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi.DEV
{
    //[ExecuteInEditMode]
    public class CloudPlanet_PUN : MonoBehaviour
    {
        public bool debug = true;

        [FormerlySerializedAs("NumberOfVertices")]
        [SerializeField]
        [Tooltip("Nombre de planètes")]
        private int numberOfVertices = 10;
        // TODO: changer nombres de vertices en fonction des joueurs connectés

        [SerializeField]
        [Tooltip("Volume de la map")]
        private float size = 10;

        [SerializeField]
        [Tooltip("Scale minimum d'une planète")]
        public int scaleMin = 30;

        [SerializeField]
        [Tooltip("Scale maximum d'une planète")]
        public int scaleMax = 70;

        [SerializeField]
        [Tooltip("Les planètes les plus répandues")]
        private GameObject basicPlanet;

        [SerializeField]
        [Tooltip("Planètes spawn joueur")]
        private GameObject spawnPlanet;

        [SerializeField]
        [Tooltip("Planète de victoire")]
        private GameObject victoryPlanet;

        [SerializeField]
        private int numberOfPlayer = 10;
        //TODO : récupérer le nombre de joueurs en jeu

        private PlanetFeature_PUN planetFeature;
        private int _seed;
        private Vertex3[] vertices;
        public GameObject[] planetTab;
        private GameObject planet;

        public int GetSeed()
        {
            _seed = Random.Range(1, 200);
            return _seed;
        }

        public void SetSeed(int value)
        {
            _seed = value;
            Debug.Log("seed dans CloudPlanet :" + _seed);
            GenerateNoise();
            //Debug.Log("scale :" + scaleMin);
            //planetFeature.CreateBiome(value);
        }

        //génération aléatoire des points 
        //TODO : set size of the planet and push points
        private Vertex3[] GenerateNoise()
        {
            int i = 0;
            Vertex3[] vertices = new Vertex3[numberOfPlayer + numberOfVertices + 1];

            //Génération aléatoire des points pour planètes spawns
            for (i = 0; i <= numberOfPlayer; i++)
            {
                var x = size * Random.Range(-0.7f, 0.7f);
                var y = size * Random.Range(-1f, -1.2f);
                var z = size * Random.Range(-0.7f, 0.7f);

                vertices[i] = new Vertex3(x, y, z);
                PhotonNetwork.Instantiate(spawnPlanet.name, new Vector3(x, y, z), Quaternion.identity);
                //planetTab[i] = planet;
                //Debug.Log("planet:" + planetTab[0]);
            }

            //Génération aléatoire des points pour planètes basics
            for (; i <= numberOfVertices; i++)
            {
                var x = size * Random.Range(-1.0f, 1.0f);
                var y = size * Random.Range(-1.0f, 1.0f);
                var z = size * Random.Range(-1.0f, 1.0f);

                vertices[i] = new Vertex3(x, y, z);
                PhotonNetwork.Instantiate(basicPlanet.name, new Vector3(x, y, z), Quaternion.identity);

            }

            PhotonNetwork.Instantiate(victoryPlanet.name, new Vector3(0, size * Random.Range(1.2f, 1.5f), 0), Quaternion.identity);

            Debug.Log("i : " + i);
            Debug.Log("numberOfPlayer:" + numberOfPlayer);
            Debug.Log("numberOfVertices :" + numberOfVertices);
            return vertices;
        }


            /* distance entre les points
            for (int i = 0; i < NumberOfVertices; i++)
            {
                if (i == 0)
                {
                    float distance = vertices[i].DistancePlanet(vertices[i].x, vertices[i].y, vertices[i].z);

                    Debug.Log("distance à i = 0  :" + distance);
                }
                else
                {
                    float distance = vertices[i].DistancePlanet(vertices[i - 1].x, vertices[i - 1].y, vertices[i - 1].z);
                    Debug.Log("distance à i > 0 :" + distance);
                    Debug.Log("Position[0]:" + vertices[i].x);
                    Debug.Log("Position[0] de i-1:" + vertices[i - 1].x);
                    Debug.Log("i = " + i);
                    Debug.Log("i-1 =" + (i-1));
                }

            }*/

/*

        public void GenerateMap(int _seed)
        {
            //génère le noise
            GenerateNoise(Type );
            //génère le type de planètes 
            planetFeature.CreateBiome(_seed);
            //instancie les objets à la surface

            //instancie les props à la surface
        }*/
    }
}