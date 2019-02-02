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

            exposer.ThirdPersonController.attractor = this;
            exposer.CharacterTrampolineScript.attractor = this;
            exposer.SelfPlayerAttractedScript.attractor = this;
            exposer.SelfOrbAttractedScript.attractor = this;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            var exposer = PlayerColliderDirectoryScript.GetExposer(collider);

            exposer.ThirdPersonController.attractor = null;
            exposer.CharacterTrampolineScript.attractor = null;
            exposer.SelfPlayerAttractedScript.attractor = null;
            exposer.SelfOrbAttractedScript.attractor = null;
        }
    }
}
