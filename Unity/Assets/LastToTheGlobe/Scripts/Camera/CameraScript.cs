using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Auteur : Abdallah

public class CameraScript : MonoBehaviour {
    [SerializeField]private string playerTag = "Player";
    [SerializeField] private float cameraOffsetOriginalX = -47.8f;
    [SerializeField] private float cameraOffsetOriginalY = 124.6787f;
    [SerializeField] private float cameraOffsetOriginalZ = 11.24362f;
    private Vector3 cameraOffsetOriginal;


    [SerializeField]
    private GameObject cameraRotatorX;

    // Use this for initialization
    void Start ()
    {
        //Récupération de la position du joueur au démarrage
        Transform player = GameObject.FindGameObjectWithTag(playerTag).transform;
        //cameraOffset = Distance entre la caméra et le joueur
        cameraOffsetOriginal = player.position - new Vector3(player.position.x, player.position.y + 3f, player.position.z - 9f);
    }

    // Update is called once per frame
    void Update()
    {

        //Récupération de la position du joueur à chaque frame
        Transform player = GameObject.FindGameObjectWithTag(playerTag).transform;
        transform.position = player.position;
        transform.rotation = player.rotation * cameraRotatorX.transform.rotation;
        transform.position -= transform.rotation * cameraOffsetOriginal;   
    }
}
