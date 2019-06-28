using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Auteur : Margot
namespace Editor
{
    public class GP_GUI : EditorWindow
    {
        public GP_GUISettings GUISettings { get; set; }
        private List<GameObject> forPrefab = new List<GameObject>();
        private GameObject planet;
        private Material mat;


        private Mesh mesh;
        private int planetResolution = 5;
        private Vector3 localUp;
        private Vector3 axeX;
        private Vector3 axeY;
        private MeshFilter[] meshFilters;
        private CreationPlanetMesh[] planetFaces;

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.HelpBox("\n                                PLANET GENERATOR                            \n\n" +
                "This is a tool to quickly generate a planet with custom parameters. \nRefer to the instructions below.\n", MessageType.None);

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
            EditorGUILayout.LabelField("Number of trees : ");
            GUISettings.NumberOfTree = EditorGUILayout.IntField(GUISettings.NumberOfTree);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WIP Number of rocks : ");
            GUISettings.NumberOfRock = EditorGUILayout.IntField(GUISettings.NumberOfRock);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Number of chests : ");
            GUISettings.NumberOfChest = EditorGUILayout.IntField(GUISettings.NumberOfChest);
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
                if (GUISettings.Name == null || GUISettings.Name == "lol")
                {
                    EditorGUILayout.HelpBox("You need to name your prefab to save it !", MessageType.Error);
                }

                CreatePrefab();
            }

            if (GUILayout.Button("WIP : Create another planet "))
            {

            }

            if (GUILayout.Button("Erase previous planet "))
            {
                ResetScene();
            }

            if (GUILayout.Button("Test mesh"))
            {
                GenerateMesh();
            }
            

            if (GUISettings.PlanetType == PlanetType.Crystal && GUISettings.NumberOfTree > 0)
            {
                EditorGUILayout.HelpBox("Can't grow any tree on a crystal planet ! \n Please, put 0", MessageType.Warning);
            }

            EditorGUILayout.HelpBox("TODO : \recalculer placement prefab rock + pivot à 0" +
                                    " \n ajouter la creation de mesh \n" +
                                    " ajouter option de tesselation en fonction du radius de la planet " +
                                    "\n générer le bouton create another (faire un offset de la planet) " +
                                    "\n remplacer ce message par des instructions " +
                                    "\nGUI plus propre", MessageType.None);
            EditorGUILayout.EndVertical();
        }

        public void GeneratePlanet()
        {
            if (GUISettings.SpawnPlanet == true)
            {
                planet = Instantiate(Resources.Load("SM_SpawnPlanet", typeof(GameObject))) as GameObject;
                forPrefab.Insert(0, planet);
            }
            else
            {
                //Create planet sphere and set scale
                planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                forPrefab.Insert(0, planet);
                Undo.RegisterCreatedObjectUndo(planet, "Create planets");
                //-------------------------------------SCALE------------------------------------------------
                planet.transform.localScale = new Vector3(GUISettings.Scale, GUISettings.Scale, GUISettings.Scale);
                //----------------------------------PLANET TYPE---------------------------------------------
                switch (GUISettings.PlanetType)
                {
                    //Planet Material
                    case PlanetType.Frozen:
                        {
                            mat = Resources.Load("M_FrozenPlanet", typeof(Material)) as Material;
                            break;
                        }
                    case PlanetType.Crystal:
                        {
                            mat = Resources.Load("M_CrystalPlanet", typeof(Material)) as Material;
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
                //----------------------------------MATERIAL------------------------------------------------
                planet.GetComponent<Renderer>().material = mat;
            }
            //-----------------------------------SPAWN CHEST-----------------------------------------------
            SpawnChest(GUISettings.NumberOfChest, planet);

            //-----------------------------------SPAWN TREES-----------------------------------------------
            SpawnTrees(GUISettings.NumberOfChest, GUISettings.NumberOfTree, planet, GUISettings.PlanetType);

            //-----------------------------------SPAWN ROCKS-----------------------------------------------
            SpawnRocks(GUISettings.NumberOfTree, GUISettings.NumberOfRock, planet, GUISettings.PlanetType);


        }

        public void CreatePrefab()
        {
            foreach (GameObject gameObject in forPrefab)
            {
                //Set the path as within the ressources folder
                string localPath = "Assets/Resources/Planet" + GUISettings.Name + ".prefab";
                PrefabUtility.SaveAsPrefabAsset(planet, localPath);
            }
        }

        public void ResetScene()
        {
            for (int i = 0; i < forPrefab.Count; i++)
            {
                DestroyImmediate(forPrefab[i]);
            }
            ClearList();
        }

        public void ClearList()
        {
            forPrefab.Clear();
        }

        private void SpawnChest(int numberOfChest, GameObject planet)
        {
            for (int i = 1; i <= numberOfChest; i++)
            {
                GameObject newChest = Instantiate(Resources.Load("Chest", typeof(GameObject))) as GameObject;
                var spawnPosition = new Vector3(0, 0, 0);

                if (GUISettings.SpawnPlanet == true)
                {
                    planet = GameObject.Find("polySurface1");
                    spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + (newChest.transform.localScale.y) + 2f);
                }
                else
                {
                    spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + (newChest.transform.localScale.y / 10) / 3f);
                }
                newChest.transform.position = spawnPosition;
                newChest.transform.LookAt(planet.transform.position);
                newChest.transform.Rotate(-90, 0, 0);
                newChest.transform.SetParent(planet.transform);
                forPrefab.Insert(i, newChest);
            }
        }

        private void SpawnTrees(int numberOfChest, int numberOfTrees, GameObject planet, PlanetType type)
        {
            string prefabTree = "Tree_" + type + "_0";
            float _randomScaleTree = Random.Range(0.05f, 0.15f);
            int randomBasicTrees;
            GameObject newTree;

            for (int i = 1; i <= GUISettings.NumberOfTree; i++)
            {

                randomBasicTrees = Random.Range(1, 5);
                var spawnPosition = new Vector3(0, 0, 0);

                newTree = Instantiate(Resources.Load(prefabTree + randomBasicTrees, typeof(GameObject))) as GameObject;

                if (GUISettings.SpawnPlanet == true)
                {
                    planet = GameObject.Find("pCube2");
                    spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x / 2) + (newTree.transform.localScale.y) + 11.5f);
                }
                else
                {
                    spawnPosition = Random.onUnitSphere * (((planet.transform.localScale.x / 2) - 0.2f) + newTree.transform.localScale.y - 0.1f) + planet.transform.position;
                }
                newTree.transform.position = spawnPosition;
                newTree.transform.LookAt(planet.transform.position);
                newTree.transform.Rotate(-90, 0, 0);
                newTree.transform.SetParent(planet.transform);
                forPrefab.Insert(i + numberOfChest, newTree);
            }
        }

        private void SpawnRocks(int numberOfTrees, int numberOfRocks, GameObject planet, PlanetType type)
        {
            //TODO : retouche les prefabs pour pas qu'on s'enfonce trop dans la planet
            string prefabRock = "P_Rock" + type + "_v";
            float _randomScaleRocks = Random.Range(0.05f, 0.15f);
            int randomBasicRocks;
            GameObject newRocks;

            for (int i = 1; i <= GUISettings.NumberOfRock; i++)
            {
                randomBasicRocks = Random.Range(1, 7);

                newRocks = Instantiate(Resources.Load(prefabRock + randomBasicRocks, typeof(GameObject))) as GameObject;
                var spawnPosition = Random.onUnitSphere * (((planet.transform.localScale.x / 2) - 0.2f) + newRocks.transform.localScale.y - 0.1f) + planet.transform.position;
                newRocks.transform.position = spawnPosition;
                newRocks.transform.LookAt(planet.transform.position);
                newRocks.transform.Rotate(-90, 0, 0);
                newRocks.transform.SetParent(planet.transform);
                forPrefab.Insert(i + numberOfTrees, newRocks);
            }
        }
        /*
        private void CreationPlanetMesh(Mesh mesh, int planetResolution, Vector3 localUp)
        {
            this.mesh = mesh;
            this.planetResolution = planetResolution;
            this.localUp = localUp;

            axeX = new Vector3(localUp.y, localUp.z, localUp.x);
            axeY = Vector3.Cross(localUp, axeX);
        }

        //Creation of vertices and tris
        /*private void CreateMesh()
        {
            Vector3[] vertices = new Vector3[planetResolution * planetResolution];
            int[] triangles = new int[(planetResolution - 1) * (planetResolution - 1) * 6];
            int index = 0;

            for (int y = 0; y < planetResolution; y++)
            {
                for (int x = 0; x < planetResolution; x++)
                {
                    int i = x + y * planetResolution;
                    Vector2 percent = new Vector2(x, y) / (planetResolution - 1);
                    Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axeX + (percent.y - .5f) * 2 * axeY;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    vertices[i] = pointOnUnitSphere;

                    //Index of vertices to create tris
                    if (x != planetResolution - 1 && y != planetResolution - 1)
                    {
                        triangles[index] = i;
                        triangles[index + 1] = i + planetResolution + 1;
                        triangles[index + 2] = i + planetResolution;

                        triangles[index + 3] = i;
                        triangles[index + 4] = i + 1;
                        triangles[index + 5] = i + planetResolution + 1;

                        index += 6;

                    }
                }

                mesh.Clear();
                //creation of vertices and tris
                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.RecalculateNormals();
            }
        }
        */
        private void Initialize()
        {
            if (meshFilters == null || meshFilters.Length == 0)
            {
                meshFilters = new MeshFilter[5];// numberMeshFilters];
            }

            //planetFaces = new CreationPlanetMesh[numberMeshFilters];

            Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

            for (int i = 0; i < 5; i++) //numberMeshFilters; i++)
            {
                if (meshFilters[i] == null)
                {
                    GameObject meshObj = new GameObject("PlanetFace");
                    //meshObj.transform.parent = transform;
                    meshObj.transform.localScale = new Vector3(10, 10, 10);

                    meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                    meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i].sharedMesh = new Mesh();
                }

                planetFaces[i] = new CreationPlanetMesh(meshFilters[i].sharedMesh, 100, directions[i]);
            }
        }


        void GenerateMesh()
        {
            Initialize();
            foreach (CreationPlanetMesh face in planetFaces)
            {
                face.CreateMesh();
            }
        }

    }
}