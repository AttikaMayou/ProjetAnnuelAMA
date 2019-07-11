using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Marhot
//Modification : Attika

//[ExecuteInEditMode]
namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet
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
        private int numberOfPlayer = 10;

        private Vector3[] vertices;
        private int[] indices;
        private Vector3[] planetSize;
        private Vector3[] positionTremplin;
        //TODO : récupérer le nombre de joueurs en jeu --> ColliderDirectoryScript.Instance.characterExposers.Count()

        private GameObject planet;

        public void Awake()
        {
            vertices = new Vector3[numberOfPlayer + numberOfVertices + 1];
            indices = new int[numberOfPlayer + numberOfVertices];
            planetSize = new Vector3[numberOfPlayer + numberOfVertices];
            positionTremplin = new Vector3[GameVariablesScript.Instance.nbreOfTremplin * numberOfVertices];
        }

        public void Update()
        {
            SetTremplinLocation();
        }

        public int[] GetIndices()
        {
            return indices;
        }

        public void SetIndices(int[] values)
        {
            indices = values;
        }
        
        public Vector3[] GetVertices()
        {
            return vertices;
        }

        public void SetVertices(Vector3[] values)
        {
            vertices = values;
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
            for (i = numberOfPlayer; i <= numberOfVertices + numberOfPlayer -1; i++)
            {
                var x = size * Random.Range(-1.0f, 1.0f);
                var y = size * Random.Range(-0.7f, 1.0f);
                var z = size * Random.Range(-1.0f, 1.0f);

                vertices[i] = new Vector3(x, y, z);
            }

            vertices[i] = new Vector3(size * Random.Range(1f, 1.2f), size * Random.Range(1f, 1.2f), size * Random.Range(1f, 1.2f));


            return vertices;
        }

        //1-4 spawn   1-22 basic planet
        private int[] GenerateIndices()
        {
            int tmp;
            int i = 0;

            for(i= 0; i<numberOfPlayer; i++)
            {
                indices[i] = (int)Random.Range(1, 4);
            }

            for(i = numberOfPlayer; i< numberOfVertices + numberOfPlayer; i++)
            {
                tmp = (int)Random.Range(1, 22);
                indices[i] = tmp == 0 ? 1 : tmp;
            }

            return indices;
        }

        //Instanciation des planets
        private void InstantiatePlanet()
        {
            string spawnPlanet = "SpawnPlanet_v";
            string basicPlanet = "Planet_v";
            string victoryPlanet = "VictoryPlanet";

            for (int i = 0; i < vertices.Length - 1 ; i++)
            {

                if (Distance(vertices[i], vertices[i + 1]) >= 150 && i != vertices.Length - 2)
                {
                    if (i < numberOfPlayer)
                    {
                        planet = Instantiate(Resources.Load(spawnPlanet + indices[i]), vertices[i], Quaternion.identity) as GameObject;
                        planetSize[i] = planet.transform.localScale;
                        continue;
                    }

                    planet = Instantiate(Resources.Load(basicPlanet + indices[i]), vertices[i], Quaternion.identity) as GameObject;
                    planetSize[i] = planet.transform.localScale;
                }
                else if(i == vertices.Length - 2)
                {
                    planet = Instantiate(Resources.Load(victoryPlanet), new Vector3(0, size * 1.2f, 0), Quaternion.identity) as GameObject;
                }
                else
                {
                    
                    continue;
                }

            }
        }

        public float Distance(Vector3 Position, Vector3 Position2)
        {
            float x = Position.x - Position2.x;
            float y = Position.y - Position2.y;
            float z = Position.z - Position2.z;
            //distance euclidienne entre 2 points de l'espace
            return Mathf.Round(Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2)));
        }


        //Cette fonction renvoie la position des tremplins pour la planète à l'Id 'planetId' de taille 'planetSize'
        private Vector3[] SetTremplinLocation()
        {
            //On créé un tableau de Vector3 qui contiendra les positions des tremplins 
            Vector3[] locations = new Vector3[GameVariablesScript.Instance.nbreOfTremplin];
            Vector3[] locationsNearest = new Vector3[GameVariablesScript.Instance.nbreOfTremplin];

            //1) On récupère x planètes les plus proches de la planète correspondante à l'ID en paramètre (x = nbreOfTremplin) 
            //
            for (int i = 0; i < indices.Length; i++)
            {
                var min = 1000f;
                var min2 = 1000f;
                var min3 = 1000f;

                for (int j = 0; j < vertices.Length - 2; j+=2)
                {
                    var minTemp = Distance(vertices[j], vertices[j + 1]);

                    if (min >= minTemp)
                    {
                        min3 = minTemp;
                        min2 = minTemp;
                        min = minTemp;
                        locationsNearest[i] = vertices[j + 1];
                    }
                    else if (min2 >= minTemp)
                    {
                        min3 = min2;
                        min2 = minTemp;
                        locationsNearest[i + 1] = vertices[j + 1];
                    }
                    else if (min3 >= minTemp)
                    {
                        min3 = minTemp;
                        locationsNearest[i + 2] = vertices[j + 1];
                    }

                    Debug.LogFormat("3 minimum : {0}, {1}, {2} à index : {3}", min, min2, min3, i);
                }

                //locations[i] = ColliderDirectoryScript.Instance.GetPlanetExposer(planetId[i]).ClosestPoint(locationsNearest[i]);
            }

            //2) Pour chaque planète trouvée : on récupère le point le plus proche de cette planète (à l'aide de la fonction Collider.ClosestPoint)
            //Pour récupérer le Collider : ColliderDirectoryScript.Instance.GetPlanetExposer(planetId) --> renvoie le collider de la planète à l'id donné

            //3) On l'ajoute au tableau de position

            return locations;
        }

    }
}
    
