using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voronoi.CloudVertices;

//Auteur : Margot

namespace Voronoi
{
    //[ExecuteInEditMode]
    public class CloudPlanet : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Nombre de planètes")]
        int NumberOfVertices = 10;
        // TODO: changer nombres de vertices en fonction des joueurs connectés

        [SerializeField]
        public int seed = 0;
        //TODO : changer seed SEULEMENT quand MasterClient click play

        [SerializeField]
        [Tooltip("Volume de la map")]
        public float size = 10;

        [SerializeField]
        [Tooltip("Les planètes les plus répandues")]
        private GameObject basicPlanet;

        //[SerializeField]
        //[Tooltip("Planètes spawn joueur")]
        //private GameObject spawnPlanet;

        //[SerializeField]
        //[Tooltip("Planète de victoire")]
        //private GameObject victoryPlanet;


        void Start()
        {
            Random.InitState(seed);

            Vertex3[] vertices = new Vertex3[NumberOfVertices];

            //Génération aléatoire des points
            for (int i = 0; i < NumberOfVertices; i++)
            {
                float x = size * Random.Range(-1.0f, 1.0f);
                float y = size * Random.Range(-1.0f, 1.0f);
                float z = size * Random.Range(-1.0f, 1.0f);

                vertices[i] = new Vertex3(x, y, z);

                Instantiate(basicPlanet, new Vector3(x, y, z), Quaternion.identity);
                // TODO: tableau de sphères (futur tableau de planètes) + planètes radius qui change en fonction de la distance avec 
                // la planète la plus proche + biomes aléatoire et spawnplanete
                //créer ID pour planete (ID spawn, ID planet, ID endplanet)

            }

            for (int i = 0; i < NumberOfVertices; i++)
            {
                if (i == 0)
                {
                    float distance = new Vertex3.distancePlanet(vertices[i].x, vertices[i].y, vertices[i].z);

                    Debug.Log("distance à i = 0  :" + distance);
                }
                else
                {
                    float distance = new distancePlanet(vertices[i - 1].x, vertices[i - 1].y, vertices[i - 1].z);
                    Debug.Log("distance à i > 0 :" + distance);
                    Debug.Log("Position[0]:" + vertices[i].x);
                    Debug.Log("Position[0] de i-1:" + vertices[i - 1].x);
                    Debug.Log("i = " + i);
                    Debug.Log("i-1 =" + (i-1));
                }

            }

        }

    }
}