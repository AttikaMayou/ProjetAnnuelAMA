using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractedScript : MonoBehaviour {

    public AttractorScript attractor;

    private Transform myTranform;

    [SerializeField]
    private Rigidbody attractedRigidbody;

    [SerializeField]
    private float selfGravity = -10f;

    public bool firstStepOnGround;

	// Use this for initialization
	void Start () {

        attractedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        attractedRigidbody.useGravity = false;
        myTranform = transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(firstStepOnGround && attractor != null)
        {
            attractor.Attractor(attractedRigidbody, myTranform, -2600f);
        }
        else if (!firstStepOnGround && attractor != null)
        {
            attractor.Attractor(attractedRigidbody, myTranform, selfGravity);
        }
        
	}
}
