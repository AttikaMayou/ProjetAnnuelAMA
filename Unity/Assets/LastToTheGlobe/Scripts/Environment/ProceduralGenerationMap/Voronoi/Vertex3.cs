using UnityEngine;

//Auteur : Margot

namespace LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi
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
            get { return Position[0]; }
            set { Position[0] = value; }
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

        public Vertex3(float x, float y, float z)
        {
            Position = new float[] { x, y, z };
        }

        public float DistancePlanet(float px, float py, float pz)
        {
            float x = Position[0] - px;
            float y = Position[1] - py;
            float z = Position[2] - pz;
            //distance euclidienne entre 2 points de l'espace
            return Mathf.Round(Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2)));
        }


    }
}
