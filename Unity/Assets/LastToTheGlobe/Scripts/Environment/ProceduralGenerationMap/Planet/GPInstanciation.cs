using UnityEngine;

//Auteur : Margot

namespace Assets.LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet
{
    public class GPInstanciation : MonoBehaviour
    {
        [SerializeField]
        private GameObject tremplin;

        [SerializeField]
        private GameObject chest;

        [SerializeField]
        private GameObject planet;

        private int _numberOfTremplin;
        private int _numberOfChest;

        private void Start()
        {
            _numberOfTremplin = Random.Range(1, 5);
            _numberOfChest = Random.Range(1, 3);
            //TODO : changer en fonction de la taille des planètes
            // faire en sorte qu'ils soient toujours vers d'autres planètes

            for (var i = 0; i <= _numberOfTremplin; i++)
            {
                var planetPosition = gameObject.transform.position;

                var spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + tremplin.transform.localScale.y - 0.02f) + planet.transform.position;
                var newTree = Instantiate(tremplin, spawnPosition, Quaternion.identity) as GameObject;

                newTree.transform.LookAt(planetPosition);
                newTree.transform.Rotate(-90, 0, 0);
                newTree.transform.parent = transform;
            }

            for (var i = 0; i <= _numberOfChest; i++)
            {
                var planetPosition = gameObject.transform.position;

                var spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + chest.transform.localScale.y - 0.02f) + planet.transform.position;
                var newTree = Instantiate(chest, spawnPosition, Quaternion.identity) as GameObject;

                newTree.transform.LookAt(planetPosition);
                newTree.transform.Rotate(-90, 0, 0);
                newTree.transform.parent = transform;
            }
        }


    }
}
