using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot

namespace Voronoi.CloudVertices
{
    //Class Vertex pour la génération des points aléatoires dans l'espace
    public class Vertex3
    {
        public float[] Position
        {
            get;
            set;
        }

        public float x
        {
            get { return Position[1]; }
            set { Position[1] = value; }
        }

        public float y
        {
            get { return Position[1];  }
            set { Position[1] = value; }
        }

        public float z
        {
            get { return Position[2];  }
            set { Position[2] = value; }
        }

        public Vertex3 (float x, float y, float z)
        {
            Position = new float[] { x, y, z };
        }


    }
}
