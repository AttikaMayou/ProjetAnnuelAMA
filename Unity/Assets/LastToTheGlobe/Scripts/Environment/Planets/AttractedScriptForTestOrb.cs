using UnityEngine;

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractedScriptForTestOrb : AttractedScript
    {
        // Start is called before the first frame update
        [SerializeField] private AttractorScript theAttractor;
        [SerializeField] private GameObject Sphere;
        void Start()
        {
            attractor = theAttractor;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.A)) return;
            Sphere.SetActive(true);
        }
    }
}
