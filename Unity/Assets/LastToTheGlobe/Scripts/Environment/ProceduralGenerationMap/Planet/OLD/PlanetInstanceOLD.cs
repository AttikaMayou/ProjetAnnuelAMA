using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot
//-------------------------------------------
// Script to generate planet's mesh
//-------------------------------------------

public class PlanetInstanceOLD : MonoBehaviour
{
    [SerializeField]
    private int resolution = 5;

    [SerializeField]
    private float radius = 1.0f ;

    [SerializeField]
    private int numberMeshFilters = 6;

    MeshFilter[] meshFilters;
    CreationPlanetMesh[] planetFaces;

    private void Start()
    {
        Initialize();
        GenerateMesh();
    }

    void Initialize()
    {
        if(meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[numberMeshFilters];
        }

        planetFaces = new CreationPlanetMesh[numberMeshFilters];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for(int i = 0; i < numberMeshFilters; i ++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("PlanetFace");
                meshObj.transform.parent = transform;
                meshObj.transform.localScale = new Vector3(radius, radius, radius);

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            planetFaces[i] = new CreationPlanetMesh(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    //faces generation
    void GenerateMesh()
    {
        foreach (CreationPlanetMesh face in planetFaces)
        { 
            face.CreateMesh();
        }
    }
 }
