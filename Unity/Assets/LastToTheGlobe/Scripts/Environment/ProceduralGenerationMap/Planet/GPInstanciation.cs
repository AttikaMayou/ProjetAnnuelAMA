using UnityEngine;
using LastToTheGlobe.Scripts.Environment;

//Auteur : Margot

namespace Assets.LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet
{
    public class GPInstanciation : MonoBehaviour
    {/*
        [SerializeField]
        private GameObject tremplin;

        //to return
        private Vector3[] tabPositionTremplin;

        private CloudPlanet cloudPlanetInstance;
        private int[] tabIndices;
        private int _numberOfTremplin;
        private GameObject planet;
        private Vector3[] size;

        private void InstantiateTremplin(Vector3[] , int[] tabIndices)
        {
            _numberOfTremplin = Random.Range(1, 5);
            //TODO : changer en fonction de la taille des planètes
            // faire en sorte qu'ils soient toujours vers d'autres planètes

            size = cloudPlanetInstance.planetSize;
            tabIndices = cloudPlanetInstance.GetIndices();

            for (var i = 0; i <= _numberOfTremplin; i++)
            {
                var planetPosition = gameObject.transform.position;

                var spawnPosition = new Vector3(0, planet.transform.localScale.y, 0);
                var newTP = Instantiate(tremplin, spawnPosition, Quaternion.identity) as GameObject;

                newTP.transform.LookAt(planetPosition);
                newTP.transform.Rotate(-90, 0, 0);
                newTP.transform.parent = transform;
            }
        }
        */

    }
}
