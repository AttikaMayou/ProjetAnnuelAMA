using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet;


//Auteur : Margot
namespace Editor
{
    public class GP_GUI : EditorWindow
    {
        public GP_GUISettings GUISettings { get; set; }
        private List<GameObject> forPrefab;
        private GameObject planet;
        private Material mat;
        private GameObject spawnPlanet;
        private AssetInstanciation_PUN spawnAsset;

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Is a spawn planet ? : ");
            GUISettings.SpawnPlanet = EditorGUILayout.Toggle(GUISettings.SpawnPlanet);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scale : ");
            GUISettings.Scale = EditorGUILayout.IntField(GUISettings.Scale);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("PlanetType : ");
            GUISettings.PlanetType = (PlanetType)EditorGUILayout.EnumPopup(GUISettings.PlanetType);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WIP Number of trees : ");
            GUISettings.NumberOfTree = EditorGUILayout.IntField(GUISettings.NumberOfTree);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WIP Number of rock : ");
            GUISettings.NumberOfRock = EditorGUILayout.IntField(GUISettings.NumberOfRock);
            EditorGUILayout.EndHorizontal();


            if (GUILayout.Button("Generate planet"))
            {
                GeneratePlanet();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prefab Name : ");
            GUISettings.Name = EditorGUILayout.TextField(GUISettings.Name);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Save planet "))
            {
                CreatePrefab();
            }

            if (GUILayout.Button("WIP Create another planet "))
            {
                ResetScene();
            }

            EditorGUILayout.EndVertical();
        }

        public void GeneratePlanet()
        {
            if(GUISettings.SpawnPlanet == true)
            {
                spawnPlanet = Instantiate(Resources.Load("SM_SpawnPlanet", typeof(GameObject))) as GameObject;
                forPrefab[0] = spawnPlanet;
            }
            else
            {
                //Create planet sphere and set scale
                planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                forPrefab.Add(planet);
                Undo.RegisterCreatedObjectUndo(planet, "Create planets");
                //SCALE
                planet.transform.localScale = new Vector3(GUISettings.Scale, GUISettings.Scale, GUISettings.Scale);

                //----------------------------------PLANET TYPE---------------------------------------------------
                switch (GUISettings.PlanetType)
                {
                    //Planet Material
                    case PlanetType.Frozen:
                        {
                            mat = Resources.Load("M_FrozenPlanet", typeof(Material)) as Material;
                            break;
                        }
                    case PlanetType.Desert:
                        {
                            mat = Resources.Load("M_DesertPlanet", typeof(Material)) as Material;
                            break;
                        }
                    case PlanetType.Basic:
                        {
                            mat = Resources.Load("M_BasicPlanet", typeof(Material)) as Material;
                            break;
                        }
                    default:
                        mat = Resources.Load("M_BasicPlanet", typeof(Material)) as Material;
                        break;
                }
                //MATERIAL
                planet.GetComponent<Renderer>().material = mat;
            }
            //SpawnObject
            //TODO : faire une liste des games objects avec instantiate resources load
            //spawnAsset.SpawnAssets(GUISettings.NumberOfTree, GUISettings.NumberOfRock, GUISettings.PlanetType, planet);
        }

        
        public void CreatePrefab()
        {
            foreach (GameObject gameObject in forPrefab)
            {
                //Set the path as within the ressources folder
                string localPath = "Assets/Resources/" + GUISettings.Name + ".prefab";
                CreateNew(gameObject, localPath);
           
            }
        }
        
        public void CreateNew(GameObject planet, string localPath)
        {
            bool success = false;
            //Create a new Prefab at the path given
            PrefabUtility.SaveAsPrefabAsset(planet, localPath, out success);
        }

        //reset object
        public void ResetScene()
        {
            DestroyImmediate(forPrefab[0]);
            forPrefab.Clear();
            //TODO : vérifier la liste vidée
        }
       
    }
}