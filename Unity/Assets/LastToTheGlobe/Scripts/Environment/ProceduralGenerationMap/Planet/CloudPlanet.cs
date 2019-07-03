using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

//Auteur : Marhot
//Modification : Attika

//[ExecuteInEditMode]
namespace LastToTheGlobe.Scripts.Environment
    {
    public class CloudPlanet : MonoBehaviour
    {
        public bool debug = true;

        //[Syncvar (hook=functName)]
        //Random.State seedServeur;

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

        private Vector3[] vertices;
        private int[] indices;
        //TODO : récupérer le nombre de joueurs en jeu --> ColliderDirectoryScript.Instance.characterExposers.Count()

        //private PlanetFeature planetFeature;
        //private int _seed;
        //private Vertex3[] planetsLocations;
        private GameObject planet;
        private PlanetClass[] planetsData;


        public void Awake()
        {
            vertices = new Vector3[numberOfPlayer + numberOfVertices + 1];
            indices = new int[numberOfPlayer + numberOfVertices];
        }

        public int[] GetIndices()
        {
            return indices;
        }

        public Vector3[] GetVertices()
        {
            return vertices;
        }

        public void GenerateMap()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GenerateLocation();
                GenerateIndices();
            }
            InstantiatePlanet();
        }

        //Génération du tableau de locations
        private Vector3[] GenerateLocation()
        {
            //Random.state = seedServeur;
            int i = 0;

            //Génération aléatoire des points pour planètes spawns
            for (i = 0; i <= numberOfPlayer; i++)
            {
                var x = size * Random.Range(-0.7f, 0.7f);
                var y = size * Random.Range(-1f, -1.2f);
                var z = size * Random.Range(-0.7f, 0.7f);

                vertices[i] = new Vector3(x, y, z);
            }

            //Génération aléatoire des points pour planètes basics
            for (; i <= numberOfVertices; i++)
            {
                var x = size * Random.Range(-1.0f, 1.0f);
                var y = size * Random.Range(-1.0f, 1.0f);
                var z = size * Random.Range(-1.0f, 1.0f);

                vertices[i] = new Vector3(x, y, z);
            }

            vertices[i] = new Vector3(size * Random.Range(1f, 1.2f), size * Random.Range(1f, 1.2f), size * Random.Range(1f, 1.2f));
            return vertices;
        }

        //1-4 spawn   1-22 basic planet
        private int[] GenerateIndices()
        {
            int i = 0;
            for(i= 0; i<numberOfPlayer; i++)
            {
                indices[i] = (int)Random.Range(1, 4);
            }

            for(; i< numberOfVertices - numberOfPlayer - 1; i++)
            {
                indices[i] = (int)Random.Range(1, 22);
            }

            return indices;
        }

        //Instanciation des planets
        private void InstantiatePlanet()
        {
            string spawnPlanet = "SpawnPlanet_v";
            string basicPlanet = "Planet_v";
            string victoryPlanet = "VictoryPlanet";

            for (int i = 0; i < vertices.Length; i++)
            {
                if (i < numberOfPlayer)
                {
                    planet = Instantiate(Resources.Load(spawnPlanet + indices[i]), vertices[i], Quaternion.identity) as GameObject;
                    continue;
                }

                if(i == vertices.Length - 1)
                {
                    planet = Instantiate(Resources.Load(victoryPlanet), vertices[i], Quaternion.identity) as GameObject;
                }
                planet = Instantiate(Resources.Load(basicPlanet + indices[i]), vertices[i], Quaternion.identity) as GameObject;

            }
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
                
    }
}
    
