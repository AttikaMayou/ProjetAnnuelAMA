using UnityEngine;
using LastToTheGlobe.Scripts.Environment;

//Auteur : Margot

namespace Assets.LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet
{
    public class GPInstanciation : MonoBehaviour
    {
        [SerializeField]
        private GameObject tremplin;

        private Vector3 tabPosition;

        private int _numberOfTremplin;

        private void Start()
        {
            _numberOfTremplin = Random.Range(1, 5);
            //TODO : changer en fonction de la taille des planètes
            // faire en sorte qu'ils soient toujours vers d'autres planètes

            //tabPosition = CloudPlanet.vert

            for (var i = 0; i <= _numberOfTremplin; i++)
            {
                var planetPosition = gameObject.transform.position;

                //var spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + tremplin.transform.localScale.y - 0.02f) + planet.transform.position;
                //var newTree = Instantiate(tremplin, spawnPosition, Quaternion.identity) as GameObject;

                /*newTree.transform.LookAt(planetPosition);
                newTree.transform.Rotate(-90, 0, 0);
                newTree.transform.parent = transform;
                */
            }
        }


    }
}
