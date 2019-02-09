using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Avatar;
using UnityEngine;


//Auteur : Abdallah


public class AttractorScript : MonoBehaviour {

    public float speedRotation = 10f;
    public Vector3 dirForce;
    public Transform selfTransform;
    private AvatarExposerScript currentAvatar;
    [SerializeField]private PlayerColliderDirectoryScript PlayerColliderDirectoryScript;

    
	public void Attractor(Rigidbody attractedRigidbody, Transform body, float Gravity)
    {
        //Donne la direction de la gravité
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        attractedRigidbody.AddForce(gravityUp * Gravity);

        //Permet de replacer l'axe vertical du perso sur l'axe de la gravité
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, speedRotation * Time.deltaTime);
        dirForce = gravityUp;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            var exposer = PlayerColliderDirectoryScript.GetExposer(collider);

            exposer.thirdPersonController.attractor = this;
            exposer.characterTrampolineScript.attractor = this;
            exposer.selfPlayerAttractedScript.attractor = this;
            exposer.selfOrbAttractedScript.attractor = this;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            var exposer = PlayerColliderDirectoryScript.GetExposer(collider);

            exposer.thirdPersonController.attractor = null;
            exposer.characterTrampolineScript.attractor = null;
            exposer.selfPlayerAttractedScript.attractor = null;
            exposer.selfOrbAttractedScript.attractor = null;
        }
    }
}
