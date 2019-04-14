using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot
//-------------------------------------------
// Script to generate planet's mesh
//-------------------------------------------

public class PlanetInstance : MonoBehaviour
{
    [SerializeField]
    private int resolution = 1;

    //[SerializeField]
    //private int numberMeshFilters = 6;

    MeshFilter[] meshFilters;
    CreationPlanetMesh[] planetFaces;

    //Can change in editor
    private void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }

    void Initialize()
    {
        if(meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        planetFaces = new CreationPlanetMesh[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for(int i = 0; i < 6; i ++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                // TODO : à modifier en fonction des planètes choisies et de leurs ID
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            planetFaces[i] = new CreationPlanetMesh(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    void GenerateMesh()
    {
        foreach (CreationPlanetMesh face in planetFaces)
        { 
            face.CreateMesh();
        }
    }
 }
