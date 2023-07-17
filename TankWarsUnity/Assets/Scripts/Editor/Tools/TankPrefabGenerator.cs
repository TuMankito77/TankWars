namespace TankWars.Editor.Tools
{
    using UnityEngine;
    using UnityEditor;
    using System.Linq;
    using System.IO;
    using TankWars.Runtime.Gameplay.Guns;
    using InputSystem;
    using TankWars.Runtime.Gameplay.Vehicles;
    using TankWars.Runtime.Gameplay.Effects;
    using System.Reflection;
    using System;

    public class TankPrefabGenerator 
    {
        private const string MOVABLE_TANK_MESHES_PATH = "Assets/Art/Models/MovableTanks/";
        private const string TANK_PREFAB_SAVE_PATH = "Assets/Prefabs/Gameplay/Tanks/";
        private const string META_FILE_EXTENSION = ".meta"; 
        private const int CANION_CHILD_INDEX = 0;
        private const int TANK_BODY_CHILD_INDEX = 1;

        private static readonly Vector3 bulletSpawnPointOffset = new Vector3(0, 0, 2); 


        [MenuItem("TankWars/Generate Tank Prefabs")]
        public static void GenerateTankPrefabs()
        {
            string[] fileNames = Directory.GetFiles(MOVABLE_TANK_MESHES_PATH)
                                 .Select(fileName => Path.GetFileName(fileName))
                                 .Where(fileName => fileName.Contains(META_FILE_EXTENSION) == false)
                                 .ToArray();

            //Get all the fbx files for the tanks and assign the corresponding components. 
            foreach(string fileName in fileNames)
            {
                Debug.Log(fileName);
                GameObject tankMesh = AssetDatabase.LoadAssetAtPath($"{MOVABLE_TANK_MESHES_PATH}{fileName}", typeof(GameObject)) as GameObject; 
                
                if(tankMesh == null)
                {
                    Debug.LogWarning($"The {fileName} cannot be loaded as a GameObject. Please make sure that all the files on the Movable Tanks folder are FBX files.");
                    continue; 
                }

                string savePath = $"{TANK_PREFAB_SAVE_PATH}{fileName.Replace(".fbx",".prefab")}";
                GameObject oldPrefab = AssetDatabase.LoadAssetAtPath(savePath, typeof(GameObject)) as GameObject; 
                
                if(oldPrefab)
                {
                    bool wasAssetDeleted = AssetDatabase.DeleteAsset(savePath);
                    Debug.Log($"{nameof(GenerateTankPrefabs)}:{nameof(wasAssetDeleted)} = {wasAssetDeleted}");
                }

                GameObject tankPrefab = SetUpComponentsAndGetPrefab(tankMesh);
                PrefabUtility.SaveAsPrefabAsset(tankPrefab, savePath);
                GameObject.DestroyImmediate(tankPrefab); 
            }
        }

        private static GameObject SetUpComponentsAndGetPrefab(GameObject originalTankReference)
        {
            GameObject tankPrefab = GameObject.Instantiate(originalTankReference);
            AttachMeshColliders(tankPrefab);
            AttachScriptComponents(ref tankPrefab);  
            return tankPrefab; 
        }

        private static void AttachMeshColliders(GameObject gameObject)
        {
            if(gameObject.GetComponent<MeshFilter>() != null)
            {
                MeshCollider meshCollider = gameObject.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true; 
            }

            if(gameObject.transform.childCount <= 0)
            {
                return; 
            }

            for(int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                AttachMeshColliders(child); 
            }
        }

        private static void AttachScriptComponents(ref GameObject prefabInstance)
        {
            GameObject tankHolder = new GameObject(prefabInstance.name);
            GameObject bulletSpawnPoint = new GameObject("Bullet Spawn Point");
            GameObject canion = prefabInstance.transform.GetChild(CANION_CHILD_INDEX).gameObject;
            GameObject tankBody = prefabInstance.transform.GetChild(TANK_BODY_CHILD_INDEX).gameObject;

            Vector3 tankBodyCenter = tankBody.GetComponent<MeshCollider>().bounds.center;
            tankHolder.transform.position = tankBodyCenter;

            Vector3 canionCenter = canion.GetComponent<MeshCollider>().bounds.center;
            bulletSpawnPoint.transform.position = canionCenter + bulletSpawnPointOffset;
            bulletSpawnPoint.transform.SetParent(canion.transform); 

            Tank tank = tankHolder.AddComponent<Tank>();
            Rigidbody tankRigidBody = tankHolder.AddComponent<Rigidbody>();
            tankRigidBody.freezeRotation = true; 
            PlayerTankInput inputManager = tankHolder.AddComponent<PlayerTankInput>();
            CollisionBounce collisionBounce = tankHolder.AddComponent<CollisionBounce>();
            GunTurret gunTurret = canion.AddComponent<GunTurret>(); 
            
            Type inputManagerType = inputManager.GetType(); 
            FieldInfo tankField = inputManagerType.GetField(PlayerTankInput.TANK_FIELD_NAME, BindingFlags.NonPublic | BindingFlags.Instance);
            tankField.SetValue(inputManager, tank);

            Type tankType = tank.GetType();
            FieldInfo gunTurretField = tankType.GetField(Tank.GUN_TURRET_FIELD_NAME, BindingFlags.NonPublic | BindingFlags.Instance);
            gunTurretField.SetValue(tank, gunTurret);

            FieldInfo rigidBodyField = typeof(Vehicle).GetField(Vehicle.VEHICLE_RIGIDBODY_FIELD_NAME, BindingFlags.NonPublic | BindingFlags.Instance);
            rigidBodyField.SetValue(tank, tankRigidBody);

            Type collissionBounceType = collisionBounce.GetType();
            FieldInfo collissionBouncerigidBodyField = collissionBounceType.GetField(CollisionBounce.RIGID_BODY_FIELD_NAME, BindingFlags.NonPublic | BindingFlags.Instance);
            collissionBouncerigidBodyField.SetValue(collisionBounce, tankRigidBody);

            Type gunTurretType = gunTurret.GetType();
            FieldInfo bulletSpawnTransformField = gunTurretType.GetField(GunTurret.BULLET_SPAWN_TRANSFORM_FIELD_NAME, BindingFlags.NonPublic | BindingFlags.Instance);
            bulletSpawnTransformField.SetValue(gunTurret, bulletSpawnPoint.transform);

            canion.transform.SetParent(tankHolder.transform);
            tankBody.transform.SetParent(tankHolder.transform);
            GameObject.DestroyImmediate(prefabInstance);
            prefabInstance = null; 
            prefabInstance = tankHolder; 
        }
    }
}

