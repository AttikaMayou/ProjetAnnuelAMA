﻿using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Margot
//Modifications : Attika

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi.DEV
{
    //[ExecuteInEditMode]
    public class CloudPlanet_PUN : MonoBehaviour
    {
        public bool debug = true;

        [FormerlySerializedAs("NumberOfVertices")]
        [Tooltip("Nombre de planètes")]
        [SerializeField]
        private int numberOfVertices = 10;
        // TODO: changer nombres de vertices en fonction des joueurs connectés

        [Tooltip("Volume de la map")]
        [SerializeField]
        private float size = 10;

        [Tooltip("Scale minimum d'une planète")]
        [SerializeField]
        public float scaleMin = 30;

        [Tooltip("Scale maximum d'une planète")]
        [SerializeField]
        public float scaleMax = 70;
        
        [Tooltip("Les planètes les plus répandues")]
        [SerializeField]
        private GameObject basicPlanet;

        [Tooltip("Planètes spawn joueur")]
        [SerializeField]
        private GameObject spawnPlanet;

        [Tooltip("Planète de victoire")]
        [SerializeField]
        private GameObject victoryPlanet;

        [SerializeField]
        private int numberOfPlayer = 10;
        //TODO : récupérer le nombre de joueurs en jeu (lobby, avant de lancer le game)

        private PlanetFeature_PUN planetFeature;
        private float planetSize = 1;
        private int _seed;
        private Vertex3[] vertices;
        private PlanetClass[] planetsClass;
        private AssetInstanciation_PUN assetInstance;
        private GameObject planet;

        private string matName;

        private void Awake()
        {
            planetsClass = new PlanetClass[500];
            InitializePlanetsClass();
        }

        private void InitializePlanetsClass()
        {
            var i = 0;
            for (; i < planetsClass.Length; i++)
            {
                planetsClass[i] = new PlanetClass();
            }
        }
        
        public void SetSeed(int value)
        {
            _seed = value;
            GenerateBiome();
            if(debug) Debug.Log("enter in SetSeed()");
        }

        //génération aléatoire des points 
        private Vertex3[] GenerateBiome()
        {
            Debug.Log("enter in GenerateBiome()");

            int i = 0;
            Vertex3[] vertices = new Vertex3[numberOfPlayer + numberOfVertices + 1];

            //Génération aléatoire des points pour planètes spawns
            for (i = 0; i <= numberOfPlayer; i++)
            {
                var x = size * Random.Range(-0.7f, 0.7f);
                var y = size * Random.Range(-1f, -1.2f);
                var z = size * Random.Range(-0.7f, 0.7f);

                //vertices[i] = new Vertex3(x, y, z);
                PhotonNetwork.Instantiate(spawnPlanet.name, new Vector3(x, y, z), Quaternion.identity);
            }

            //Génération aléatoire des points pour planètes basics
            for (i = 0; i <= numberOfVertices; i++)
            {
                planetSize = Random.Range(scaleMin, scaleMax);
                var x = size * Random.Range(-1.0f, 1.0f);
                var y = size * Random.Range(-1.0f, 1.0f);
                var z = size * Random.Range(-1.0f, 1.0f);

                //vertices[i] = new Vertex3(x, y, z);

                //newPlanet.planetLocation = vertices[i];
                var planetGameObject = PhotonNetwork.Instantiate(basicPlanet.name, new Vector3(x, y, z), Quaternion.identity);
                planetGameObject.transform.localScale = new Vector3(planetSize, planetSize, planetSize);

                var planetClass = planetsClass[i];
                planetClass.gameObjectPlanet = planetGameObject;
                //assignation d'un biome
                planetClass.planetType = PlanetFeature_PUN.CreateBiome(basicPlanet, out matName);
                Material mat = Resources.Load(matName, typeof(Material)) as Material;
                planetClass.gameObjectPlanet.GetComponent<Renderer>().material = mat;
                //instanciation assets
                //AssetInstanciation_PUN asset = planetClass.gameObjectPlanet.GetComponent<AssetInstanciation_PUN>();
                //asset.type = (int)planetClass.planetType;
                //asset.SpawnAssets();
            }

            PhotonNetwork.Instantiate(victoryPlanet.name, new Vector3(0, size * Random.Range(1f, 1.2f), 0), Quaternion.identity);
            victoryPlanet.transform.localScale = new Vector3(planetSize / 2, planetSize / 2, planetSize / 2);

            return vertices;
        }


        //TODO : check if planet is in another 
    }
}