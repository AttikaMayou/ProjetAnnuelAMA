using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfAttractorSetter : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private AttractorScript planetAttracting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        planetAttracting = player.GetComponent<AttractedScript>().attractor;
        this.GetComponent<AttractedScript>().attractor = planetAttracting;
        this.GetComponent<OrbManager>().planetCenterPoint = planetAttracting;
    }
}
