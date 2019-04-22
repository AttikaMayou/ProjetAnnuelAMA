using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using LastToTheGlobe.Scripts;
using Photon.Pun;
using UnityEngine.Serialization;

//Auteur : Margot
//Modifications : Attika

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi
{
    //[ExecuteInEditMode]
    public class CloudPlanet : MonoBehaviour
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
        [Tooltip("Les planètes les plus répandues")]
        private GameObject basicPlanet;

        [SerializeField]
        [Tooltip("Planètes spawn joueur")]
        private GameObject spawnPlanet;

        [SerializeField]
        private int numberOfPlayer = 10;
        //TODO : récupérer le nombre de joueurs en jeu

        [SerializeField]
        [Tooltip("Planète de victoire")]
        private GameObject victoryPlanet;

        private int _seed;

        private void Start()
        {
            //TODO : vérifier que ça ne change que lorsque la room se crée et pas à chaque join room
            _seed = Random.Range(1, 200);
            if(debug) Debug.Log("Seed is : " + _seed);
        }
        
        public int GetSeed()
        {
            return _seed;
        }

        public void SetSeed(int value)
        {
            _seed = value;
            GenerateMap();
        }

        private void GenerateMap()
        {
            var vertices = new Vertex3[numberOfVertices];

            //Génération aléatoire des points pour planètes basics
            for (int i = 0; i < numberOfVertices; i++)
            {
                var x = size * Random.Range(-1.0f, 1.0f);
                var y = size * Random.Range(-1.0f, 1.0f);
                var z = size * Random.Range(-1.0f, 1.0f);

                vertices[i] = new Vertex3(x, y, z);

                Instantiate(basicPlanet, new Vector3(x, y, z), Quaternion.identity);

            }

            //Génération aléatoire des points pour planètes spawns
            for (var i = 0; i < numberOfPlayer; i++)
            {
                var x = size * Random.Range(-0.7f, 0.7f);
                var y = size * Random.Range(-1f, -1.2f);
                var z = size * Random.Range(-0.7f, 0.7f);

                vertices[i] = new Vertex3(x, y, z);

                Instantiate(spawnPlanet, new Vector3(x, y, z), Quaternion.identity);

            }

            Instantiate(victoryPlanet, new Vector3(0, size * Random.Range(1.2f, 1.5f), 0), Quaternion.identity);
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