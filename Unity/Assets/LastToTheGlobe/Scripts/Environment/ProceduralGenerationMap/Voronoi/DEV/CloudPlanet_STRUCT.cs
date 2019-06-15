using UnityEngine;
using UnityEngine.Serialization;
using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet;

//Auteur : Margot

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi.DEV
{
    public class CloudPlanet_STRUCT : MonoBehaviour
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
        //private GameObject[] planets;
        private PlanetClass[] planetsData;
        private string matName;


        /*public int GetSeed()
{
    Random.seed
    _seed = Random.Range(1, 200);
    return _seed;
}*/
        public void SetSeed(int seed)
        {
            GenerateMap();
        }

        public void GenerateMap()//int value)
        {

            SetPlanetStruct();

            //for (int j = 0; j <= numberOfVertices; j++)
            //{
                Instantiate(spawnPlanet, new Vector3(0,0,0), Quaternion.identity);
            //}

            Instantiate(victoryPlanet, new Vector3(0, size * Random.Range(1.2f, 1.5f), 0), Quaternion.identity);

        }

        //génération aléatoire des points 
        //TODO : set size of the planet and push points
        private void SetPlanetStruct()
        {

            //Random.state = seedServeur;
            int i = 0;

            //Génération aléatoire des points pour planètes spawns
            for (i = 0; i <= numberOfPlayer; i++)
            {
                var x = size * Random.Range(-0.7f, 0.7f);
                var y = size * Random.Range(-1f, -1.2f);
                var z = size * Random.Range(-0.7f, 0.7f);

                planetsData[i].planetID = i;
                planetsData[i].planetLocation = new Vector3(x, y, z);
                planetsData[i].gameObjectPlanet = spawnPlanet;
            }


            //Génération aléatoire des points pour planètes basics
            for (; i <= numberOfVertices; i++)
            {
                var x = size * Random.Range(-1.0f, 1.0f);
                var y = size * Random.Range(-1.0f, 1.0f);
                var z = size * Random.Range(-1.0f, 1.0f);

                planetsData[i].planetID = i;

                planetsData[i].planetLocation = new Vector3(x, y, z);
                planetsData[i].radiusPlanet = (float)Random.Range(scaleMin, scaleMax);

                planetsData[i].planetType = PlanetFeature_PUN.CreateBiome(basicPlanet, out matName);

                planetsData[i].gameObjectPlanet = basicPlanet;

                Material mat = Resources.Load(matName, typeof(Material)) as Material;
                planetsData[i].gameObjectPlanet.GetComponent<Renderer>().material = mat;


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
