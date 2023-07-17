namespace TankWars.Runtime.Test
{
    using UnityEngine;
    using UnityEditor;
    using TankWars.Runtime.Gameplay.Vehicles;
    using TankWars.Runtime.Gameplay.Guns;

    public class EditorWindowTest : EditorWindow
    {
        private const string WINDOW_NAME = "Tank Material Generator";

        [SerializeField]
        private Shader shader = null;

        [SerializeField]
        private GameObject testGameObject = null;

        [SerializeField]
        private Material testMaterial = null;

        [SerializeField]
        private ModelImporter modelImporter = null;     

        #region Unity Methods

        [MenuItem("TankWars/Tools/TankMaterialGenerator")]
        public static void ShowWindow()
        {
            GetWindow<EditorWindowTest>(WINDOW_NAME); 
        }

        private void OnGUI()
        {
            SerializedObject serializeObject = new SerializedObject(this);

            SerializedProperty property = serializeObject.FindProperty(nameof(shader));
            SerializedProperty gameObjectProperty = serializeObject.FindProperty(nameof(testGameObject));
            SerializedProperty materialProperty = serializeObject.FindProperty(nameof(testMaterial));

            EditorGUILayout.PropertyField(property, new GUIContent("Shader"));
            EditorGUILayout.PropertyField(gameObjectProperty, new GUIContent("Game object"));
            EditorGUILayout.PropertyField(materialProperty, new GUIContent("Material"));

            if (property != null)
            {
                shader = property.objectReferenceValue as Shader;
            }

            if (gameObjectProperty != null)
            {
                testGameObject = gameObjectProperty.objectReferenceValue as GameObject;
            }

            if (materialProperty != null)
            {
                testMaterial = materialProperty.objectReferenceValue as Material;
            }

            if (GUILayout.Button("Assign Material"))
            {
                /*if(testMaterial == null || testGameObject == null)
                {
                    return; 
                }*/
                
                string path = "Assets/Art/Models/MovableTanks/TankA.fbx"/*AssetDatabase.GetAssetPath(testGameObject)*/;
                Debug.Log(path);

                GameObject tankMeshReference = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                
                
                if(tankMeshReference == null)
                {
                    return; 
                }

                //Instantiate the fbx file 
                GameObject tankMeshes = Instantiate(tankMeshReference);
                //Attach a mesh collider to each of the meshes on the fbx game object instantiated
                AttachMeshColliders(tankMeshes);
                //Create an empty GameObject to hold the tank
                GameObject tankHolder = new GameObject("TankTest");
                //Move the GameObject created to the middles of the mesh collider of the tank body
                GameObject tankBody = null;
                GameObject tankCanion = null; 
                for (int i = 0; i < tankMeshes.transform.childCount; i++)
                {
                    if(tankMeshes.transform.GetChild(i).gameObject.name == "TankA_TankBody")
                    {
                        tankBody = tankMeshes.transform.GetChild(i).gameObject; 
                    }

                    if (tankMeshes.transform.GetChild(i).gameObject.name == "TankA_Canion")
                    {
                        tankCanion = tankMeshes.transform.GetChild(i).gameObject; 
                    }
                }

                if(tankBody)
                {
                    Vector3 centerPoint = tankBody.GetComponent<MeshCollider>().bounds.center;
                    tankHolder.transform.position = centerPoint; 
                }
                //Attach all the components needed to the GameObject created. 
                tankHolder.AddComponent<Tank>();
                //Attach the canion scricts to the canion mesh 
                tankCanion.AddComponent<GunTurret>();
                //Add a bullet spawn transform. 
                //Make the tank meshes child on the tank holder
                tankMeshes.transform.SetParent(tankHolder.transform);
                //Save the game asset. 
                string localPath = "Assets/Prefabs/TestTank.prefab";
                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath); 
                PrefabUtility.SaveAsPrefabAsset(tankHolder, localPath); 
                /*MeshRenderer meshRenderer = testGameObject.GetComponent<MeshRenderer>(); 

                if(meshRenderer == null)
                {
                    Debug.LogError("The gameobject does not have a mesh renderer that can be used to assign the material.");
                    return;
                }

                meshRenderer.sharedMaterial = testMaterial;

                EditorUtility.SetDirty(testGameObject);
                PrefabUtility.RecordPrefabInstancePropertyModifications(testGameObject); */

                /*Material newMaterial = new Material(shader);
                newMaterial.SetColor("Material_Color", Color.red);
                AssetDatabase.CreateAsset(newMaterial, "Assets/TankA-TiledTexture.mat");

                MeshRenderer meshRenderer = testGameObject.GetComponent<MeshRenderer>();

                if (meshRenderer == null)
                {
                    Debug.LogError("The game object does not have a MeshRenderer coponent.");
                    return; 
                }

                meshRenderer.sharedMaterial = newMaterial;*/
            }
        }

        #endregion
    
        private void AttachMeshColliders(GameObject tankMeshContainer)
        {
            for(int i = 0; i < tankMeshContainer.transform.childCount; i ++)
            {
                GameObject child = tankMeshContainer.transform.GetChild(i).gameObject;

                if (child.GetComponent<MeshFilter>())
                {
                    MeshCollider meshCollider = child.AddComponent<MeshCollider>();
                    meshCollider.convex = true; 
                }

                if(child.transform.childCount < 0)
                {
                    continue;
                }
                    
                AttachMeshColliders(child); 
            }
        }
    }
}
