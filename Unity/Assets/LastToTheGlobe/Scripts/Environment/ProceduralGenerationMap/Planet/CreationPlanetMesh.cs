using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot
//-----------------------------
//Script for generate planet's tris
//-----------------------------


public class CreationPlanetMesh
{
    Mesh mesh;
    int planetResolution = 1;
    //actual up of the face
    Vector3 localUp;
    Vector3 axeX;
    Vector3 axeY;

    public CreationPlanetMesh(Mesh mesh, int planetResolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.planetResolution = planetResolution;
        this.localUp = localUp;

        axeX = new Vector3(localUp.y, localUp.z, localUp.x);
        axeY = Vector3.Cross(localUp, axeX);
    }

    //Creation of vertices and tris
    public void CreateMesh()
    {
        Vector3[] vertices = new Vector3[planetResolution * planetResolution];
        int[] triangles = new int[(planetResolution - 1) * (planetResolution - 1) * 6];
        int i = 0;
        int index = 0;

        for (int y = 0; y < planetResolution; y++)
        {
            for (int x = 0; x < planetResolution; x++)
            {
                //i = x + y * planetResolution;
                Vector2 percent = new Vector2(x, y) / (planetResolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axeX + (percent.y - .5f) * 2 * axeY;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitCube;

                //Index of vertices to create tris
                //if(x != planetResolution - 1 && y != planetResolution - 1)
                //{
                    triangles[index + 0] = i;
                    triangles[index + 1] = i + planetResolution + 1 ;
                    triangles[index + 2] = i + 1;

                    triangles[index + 3] = i + 1;
                    triangles[index + 4] = i + planetResolution + 1;
                    triangles[index + 5] = i + planetResolution + 2;

                    index += 6;
                    i++;
                //}
            }

            mesh.Clear();
            //creation of vertices and tris
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }
    }       
                
}
