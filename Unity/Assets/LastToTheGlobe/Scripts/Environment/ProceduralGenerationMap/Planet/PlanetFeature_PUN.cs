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
        public static PlanetType CreateBiome(GameObject planet, out string mat) 
        {
            var type = (int)Random.Range(1f, 4f); 
            
            switch ((PlanetType)type)
            {
                //Planet Material
                case PlanetType.Frozen:
                {
                    mat = "M_FrozenPlanet";
                    break;
                }
                case PlanetType.Desert:
                {
                    mat = "M_DesertPlanet";
                    break;
                }
                case PlanetType.Basic:
                {
                    mat = "M_BasicPlanet";
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
