 using UnityEngine;
using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi;
 using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi.DEV;

//Auteur : Margot
//Modifications : Attika

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet
{
    public class PlanetFeature_PUN 
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
        public static PlanetType CreateBiome(GameObject planet, out string mat)// int _seed)
        {
            //_indexRandom = _seed % 3;
            var type = (int)Random.Range(1f, 4f); 
            //myType = (PlanetType)Random.Range(1f, 3f);//(PlanetType)_indexRandom;
            
            switch ((PlanetType)type)
            {
                //Planet Material
                case PlanetType.Frozen:
                {
                    //var matFrozen = Resources.Load("M_FrozenPlanet", typeof(Material)) as Material;
                    mat = "M_FrozenPlanet";
                    //planet.GetComponent<Renderer>().material = matFrozen;
                    break;
                }
                case PlanetType.Desert:
                {
                    mat = "M_DesertPlanet";
                    //planet.GetComponent<Renderer>().material = matDesert;
                    break;
                }
                case PlanetType.Basic:
                {
                    mat = "M_BasicPlanet";
                        //planet.GetComponent<Renderer>().material = matBasic;
                    break;
                }
                default:
                    mat = "M_BasicPlanet";
                    break;
            }

            return (PlanetType) type;
      

        }

    }
}
