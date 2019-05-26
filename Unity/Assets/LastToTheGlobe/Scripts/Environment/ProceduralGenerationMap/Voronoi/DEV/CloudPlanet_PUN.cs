using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet;
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
        public float scaleMin = 30;

        [SerializeField]
        [Tooltip("Scale maximum d'une planète")]
        public float scaleMax = 70;

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
        private float planetSize = 1;
        private int _seed;
        private Vertex3[] vertices;
        //public GameObject[] planetTab;
        private PlanetStruct newPlanet;

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
        }

        //génération aléatoire des points 
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
                GameObject newPlanet = PhotonNetwork.Instantiate(spawnPlanet.name, new Vector3(x, y, z), Quaternion.identity);
            }

            //Génération aléatoire des points pour planètes basics
            for (; i <= numberOfVertices; i++)
            {

                planetSize = Random.Range(scaleMin, scaleMax);
                var x = size * Random.Range(-1.0f, 1.0f);
                var y = size * Random.Range(-1.0f, 1.0f);
                var z = size * Random.Range(-1.0f, 1.0f);

                vertices[i] = new Vertex3(x, y, z);

                newPlanet.planetLocation = vertices[i];
                newPlanet.gameObjectPlanet = PhotonNetwork.Instantiate(basicPlanet.name, new Vector3(x, y, z), Quaternion.identity);
                newPlanet.gameObjectPlanet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);


                //assignation d'un biome
                //planet.planetType = planetFeature.CreateBiome(basicPlanet);

            }

            victoryPlanet = PhotonNetwork.Instantiate(victoryPlanet.name, new Vector3(0, size * Random.Range(1f, 1.2f), 0), Quaternion.identity);
            victoryPlanet.transform.localScale = new Vector3(planetSize / 2, planetSize / 2, planetSize / 2);

            return vertices;
        }


        //TODO : check if planet is in another 
    }
}