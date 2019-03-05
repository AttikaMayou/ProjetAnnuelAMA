using UnityEngine;

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractedScriptForTestOrb : AttractedScript
    {
        // Start is called before the first frame update
        [SerializeField] private AttractorScript theAttractor;
        void Start()
        {
            attractor = theAttractor;
        }
    }
}
