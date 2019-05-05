using UnityEngine;
using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi;

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

        [SerializeField]
        private CloudPlanet environmentController;

        public PlanetType myType = PlanetType.Basic;

        private int _scaleMax;
        private int _scaleMin;

        private int _seed;
        private float _scale;
        private int _indexRandom;

        public void GeneratePlanetFeature(int _seed, int _scaleMax, int _scaleMin)
        {
            Debug.Log("seed dans PlanetFeature :" + _seed);
            CreateBiome();
        }

        private int GetSeedFromCloudPlanet()
        {
            _seed = environmentController.GetSeed();
            return _seed;
        }

        private int GetScaleMaxFromCloudPlanet()
        {
            _scaleMax = environmentController.scaleMax;
            return _scaleMax;
        }

        private int GetScaleMinFromCloudPlanet()
        {
            _scaleMin = environmentController.scaleMax;
            return _scaleMin;
        }

        private void CreateBiome()
        {
            _indexRandom = _seed % 3; // (int)Random.Range(0, 3);
            myType = (PlanetType)_indexRandom;

            _scale = Random.Range(_scaleMin, _scaleMax);
            planet.transform.localScale = new Vector3(_scale, _scale, _scale);

            Debug.Log("seed dans planetFeature :" + _seed);

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
