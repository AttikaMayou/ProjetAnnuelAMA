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
        private GameObject planet;
        Vector3[] lookAt;

        public void Awake()
        {
            vertices = new Vector3[numberOfPlayer + numberOfVertices + 1];
            indices = new int[numberOfPlayer + numberOfVertices];
            lookAt = new Vector3[numberOfPlayer + numberOfVertices];
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
                        lookAt[i] = vertices[i];
                        continue;
                    }

                    planet = Instantiate(Resources.Load(basicPlanet + indices[i]), vertices[i], Quaternion.identity) as GameObject;
                    lookAt[i] = vertices[i];
                }
                else if(i == vertices.Length - 2)
                {
                    planet = Instantiate(Resources.Load(victoryPlanet), new Vector3(0, size * 1.2f, 0), Quaternion.identity) as GameObject;
                }

            }

            for(int i = numberOfPlayer; i < vertices.Length - 3; i++)
            {
                Vector3 tremplinLocation = SetTremplinLocation(i);
                GameObject tremplin = Instantiate(Resources.Load("Jumper"), tremplinLocation, Quaternion.identity) as GameObject;
                //set l'orientation
                tremplin.transform.LookAt(vertices[i], Vector3.down);
                tremplin.transform.Rotate(90, 0, 0);
                //Debug.LogFormat("lookAt du tremplin de location {0} == {1}", tremplinLocation, lookAt[i]);
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
        private Vector3 SetTremplinLocation(int planetId)
        {
            //On créé un tableau de Vector3 qui contiendra les positions des tremplins 

            //1) On récupère x planètes les plus proches de la planète correspondante à l'ID en paramètre (x = nbreOfTremplin) 
            //

            var min = 1000f;
            var j = 0;

            for (int i = numberOfPlayer; i < indices.Length; i++)
            {
                if(i == planetId)
                {
                    continue;
                }

                var minTemp = Distance(vertices[planetId], vertices[i]);

                if (minTemp <= min)
                {
                    min = minTemp;
                    j = i;
                }
            }

            return  ColliderDirectoryScript.Instance.GetPlanetExposer(planetId).planetGroundCollider.ClosestPoint(vertices[j]);
           
            //2) Pour chaque planète trouvée : on récupère le point le plus proche de cette planète (à l'aide de la fonction Collider.ClosestPoint)
            //Pour récupérer le Collider : ColliderDirectoryScript.Instance.GetPlanetExposer(planetId) --> renvoie le collider de la planète à l'id donné

            //3) On l'ajoute au tableau de position
        }

    }
}
    
