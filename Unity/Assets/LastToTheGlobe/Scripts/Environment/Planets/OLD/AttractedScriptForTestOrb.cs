using Assets.LastToTheGlobe.Scripts.Environment.Planets;
using Assets.LastToTheGlobe.Scripts.Weapon.Orb;
using LastToTheGlobe.Scripts.Weapon.Orb;
using UnityEngine;

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractedScriptForTestOrb : AttractedScript
    {
        // Start is called before the first frame update
        [SerializeField] private AttractorScript theAttractor;
        [SerializeField] private GameObject sphere;
        [SerializeField] private OrbManager _om;
        [SerializeField] private float _timeElapsed;
        [SerializeField] private bool _launched;
        void Start()
        {
            Attractor = theAttractor;
        }

        private void Update()
        {
            
            if (Input.GetKey(KeyCode.A))
            {
                _launched = true;
                _timeElapsed = _timeElapsed + Time.deltaTime;
                
            }
            
            if (_timeElapsed >= 1.5f && _launched && Input.GetKeyUp(KeyCode.A))
            {
                print("lol");
                _om.charged = true;
                sphere.SetActive(true);
                _timeElapsed = 0;
                _launched = false;
            }
            if(_launched && Input.GetKeyUp(KeyCode.A))
            {
                print("lol_2");
                sphere.SetActive(true);
                _timeElapsed = 0;
                _launched = false;
            }
            
        }
    }
}
