using UnityEngine;

//Auteur : Margot
//Modifications : Attika

public enum PlanetType
{
    Basic,
    Frozen,
    Desert
}

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet
{
    public class PlanetFeature : MonoBehaviour
    {
        [SerializeField]
        private GameObject planet;

        public PlanetType myType = PlanetType.Basic;
        
        private float _scale;
        private int _indexRandom;

        //TODO : transformer planet en prefab

        private void Start()
        {
            //Planet tag
            _indexRandom = (int)Random.Range(0, 3);
            myType = (PlanetType)_indexRandom;
            //planet.tag = tags[indexRandom];
        
            //Size Planet
            _scale = Random.Range(20f, 50f);
            planet.transform.localScale = new Vector3(_scale, _scale, _scale);

            switch (myType)
            {
                //Planet Material
                case PlanetType.Frozen:
                {
                    var matFrozen = Resources.Load("M_FrozenPlanet", typeof(Material)) as Material;
                    planet.GetComponent<Renderer>().material = matFrozen;
                    break;
                }
                case PlanetType.Desert:
                {
                    var matDesert = Resources.Load("M_DesertPlanet", typeof(Material)) as Material;
                    planet.GetComponent<Renderer>().material = matDesert;
                    break;
                }
                case PlanetType.Basic:
                {
                    var matBasic = Resources.Load("M_BasicPlanet", typeof(Material)) as Material;
                    planet.GetComponent<Renderer>().material = matBasic;
                    break;
                }
            }


        }

    }
}
