 using UnityEngine;
using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi;
 using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi.DEV;

//Auteur : Margot
//Modifications : Attika

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet
{
    public class PlanetFeature_PUN : MonoBehaviour
    {
        [SerializeField]
        private GameObject planet;

        [SerializeField]
        private CloudPlanet_PUN environmentController;

        public PlanetType myType = PlanetType.Basic;

        private float _scaleMax;
        private float _scaleMin;
        private int _indexRandom;

        //retourne le type de planet et set son material
        public PlanetType CreateBiome()// int _seed)
        {
            //_indexRandom = _seed % 3;

            myType = (PlanetType)Random.Range(1f, 3f);//(PlanetType)_indexRandom;

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

            return myType;


        }

    }
}
