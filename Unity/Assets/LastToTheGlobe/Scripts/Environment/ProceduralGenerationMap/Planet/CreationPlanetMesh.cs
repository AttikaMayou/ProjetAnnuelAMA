using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot
//-----------------------------
//Script for generate planet's mesh
//-----------------------------


public class CreationPlanetMesh
{
    Mesh mesh;
    int planetResolution;
    //actual up of the face
    Vector3 localUp;
    Vector3 axeX;
    Vector3 axeY;

    public void PlanetFaces(Mesh mesh, int planetResolution, Vector3 localUp)
    {
        axeX = new Vector3(localUp.y, localUp.z, localUp.x);
        axeY = Vector3.Cross(localUp, axeX);
    }

    public void CreateMesh()
    {
        Vector3[] vertices = new Vector3[planetResolution * planetResolution];
        int[] triangles = new int[(planetResolution - 1) * (planetResolution - 1) * 6];
        Vector3 pointOnUnitCube;

        for (int y = 0; y < planetResolution; y++)
        {
            for (int x = 0; x < planetResolution; x++)
            {
                int i = x + y * planetResolution;
                Vector2 percent = new Vector2(x, y) / (planetResolution - 1);
                
                vertices[i] = pointOnUnitCube;

                if(x != planetResolution -1 && y != planetResolution -1)
                {

                }
            }
        }
    }       
                
}

Vector3 pointOnUnitCube = localUp + ((percent.x - 0.5f) * 2 * axeX) + (percent.y - 0.5f) * 2 * axeY));