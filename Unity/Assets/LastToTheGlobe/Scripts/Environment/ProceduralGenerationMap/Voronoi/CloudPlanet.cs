using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Marhot
//Modification : Attika

//[ExecuteInEditMode]
    namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi
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
            //TODO : récupérer le nombre de joueurs en jeu --> ColliderDirectoryScript.Instance.characterExposers.Count()

            //private PlanetFeature planetFeature;
            //private int _seed;
            //private Vertex3[] planetsLocations;
            private GameObject[] planets;
            private PlanetClass[] planetsData;


        public void SetSeed()
        {
            GeneratePlanetsLocations();
        }

            //public void start()//int value)
            //{

                //CloudPlanet.vertices = new Vertex3[numberOfPlayer + numberOfVertices + 1];
                //GeneratePlanetsLocations(ref planetsLocations); // new Vertex3[numberOfPlayer + numberOfVertices + 1];
                //GeneratePlanetsLocations();
                //_seed = value;
                //Debug.Log("seed dans CloudPlanet :" + _seed);

                //Debug.Log("scale :" + scaleMin);
                //planetFeature.CreateBiome(value);
            //}

            //génération aléatoire des points 
            //TODO : set size of the planet and push points
            private void GeneratePlanetsLocations()
            {

                //Random.state = seedServeur;
                int i = 0;

                //Génération aléatoire des points pour planètes spawns
                for (i = 0; i <= numberOfPlayer; i++)
                {
                    var x = size * Random.Range(-0.7f, 0.7f);
                    var y = size * Random.Range(-1f, -1.2f);
                    var z = size * Random.Range(-0.7f, 0.7f);

                //planetsLocations[i] = new Vertex3(x, y, z);
                    planets[i] = Instantiate(spawnPlanet, new Vector3(x, y, z), Quaternion.identity);
                    //planet(Tab[i] = planet;
                    //Debug.Log("planet:" + planetTab[0]);
                }

                //Génération aléatoire des points pour planètes basics
                for (; i <= numberOfVertices; i++)
                {
                    var x = size * Random.Range(-1.0f, 1.0f);
                    var y = size * Random.Range(-1.0f, 1.0f);
                    var z = size * Random.Range(-1.0f, 1.0f);

                    //planetsLocations[i] = new Vertex3(x, y, z);
                   planets[i] = Instantiate(basicPlanet, new Vector3(x, y, z), Quaternion.identity);
                }

                planets[i] = Instantiate(victoryPlanet, new Vector3(0, size * Random.Range(1.2f, 1.5f), 0), Quaternion.identity);
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
